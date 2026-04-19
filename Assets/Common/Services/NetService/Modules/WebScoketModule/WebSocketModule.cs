using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace Common.Services.Net.Modules
{
    public class WebSocketModule
    {
        private ClientWebSocket _WebSocket;
        private Dictionary<string, List<Action<string>>> _handlers = new();
        private Dictionary<string, TaskCompletionSource<string>> _pendingRequests = new();


        private string _hostAddress;
        private bool _isConnected;

        public event Action onConnect;
        public event Action onDisconnect;

        private CancellationTokenSource _receiveCts;
        private Task _receiveTask;

        public bool IsConnected => _isConnected;

        public static async Task<ClientWebSocket> CreateConnectionTo(
            string hostAddress,
            int port,
            string[] keys = null,
            CancellationToken ct = default)
        {
            var ws = new ClientWebSocket();

            if (keys != null && keys.Length > 0)
            {
                foreach (var key in keys)
                {
                    ws.Options.SetRequestHeader("X-Key", key);
                }
            }

            var uri = new Uri($"ws://{hostAddress}:{port}/ws");

            try
            {
                await ws.ConnectAsync(uri, ct);
                return ws;
            }
            catch (OperationCanceledException)
            {
                ws.Dispose();
                throw;
            }
            catch (Exception ex)
            {
                ws.Dispose();
                throw new Exception($"Failed to connect to session server: {uri}", ex);
            }
        }
        public async Task ReplaceSessionSocketAsync(ClientWebSocket newSocket)
        {
            var oldCts = _receiveCts;
            _receiveCts = new CancellationTokenSource();


            if (oldCts != null)
            {
                oldCts.Cancel();
                try
                {
                    if (_receiveTask != null)
                        await _receiveTask; // ВАЖНО: дождаться завершения старого receive loop
                }
                catch
                {
                    // ignore cancellation / receive errors
                }
                finally
                {
                    oldCts.Dispose();
                }
            }
            var oldSocket = _WebSocket;

            if (oldSocket != null)
            {
                try
                {
                    if (oldSocket.State == WebSocketState.Open ||
                        oldSocket.State == WebSocketState.CloseReceived)
                    {
                        await oldSocket.CloseAsync(
                            WebSocketCloseStatus.NormalClosure,
                            "Replaced by new session socket",
                            CancellationToken.None
                        );
                    }
                }
                catch
                {
                    // socket мог уже умереть — это нормально
                }
                finally
                {
                    oldSocket.Dispose();
                }
            }

            _isConnected = false;

            // 4. Атомарная замена сокета
            _WebSocket = newSocket;

            // 5. Запуск нового receive loop
            StartReceiveLoop(_WebSocket);

            // 6. ВАЖНО: уведомить систему, что соединение восстановлено
            _isConnected = true;
            onConnect?.Invoke();
        }
        private async Task ReceiveLoop(ClientWebSocket socket, CancellationToken ct)
        {
            var buffer = new byte[8192];

            while (socket != null &&
                   socket.State == WebSocketState.Open &&
                   !ct.IsCancellationRequested)
            {
                try
                {
                    var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), ct);

                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        _isConnected = false;
                        onDisconnect?.Invoke();
                        break;
                    }

                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        var json = Encoding.UTF8.GetString(buffer, 0, result.Count);
                        HandleMessage(json);
                    }
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    Debug.LogError("Receive error: " + ex.Message);
                }
            }
        }

        public void setServerAdress(string adress)
        {
            if (string.IsNullOrEmpty(adress)) throw new ArgumentNullException();
            _hostAddress = adress;
        }

        private void StartReceiveLoop(ClientWebSocket socket)
        {
            _receiveCts?.Cancel();
            _receiveCts = new CancellationTokenSource();

            _receiveTask = Task.Run(() => ReceiveLoop(socket, _receiveCts.Token));
        }

        public async Task<bool> tryConnect(string AccessToken)
        {
            _WebSocket = new ClientWebSocket();
            var cts = new CancellationTokenSource(15000);

            try
            {
                await _WebSocket.ConnectAsync(new Uri($"ws://{_hostAddress}?token={AccessToken}"), cts.Token);

                if (_WebSocket.State == WebSocketState.Open)
                {
                    Console.WriteLine("Connected!");
                    _isConnected = true;
                    onConnect?.Invoke();

                    StartReceiveLoop(_WebSocket);

                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Connection failed: " + ex.Message);
            }

            return false;
        }


        public async Task Disconnect()
        {
            if (_WebSocket == null) return;

            try
            {
                if (_WebSocket.State == WebSocketState.Open ||
                    _WebSocket.State == WebSocketState.CloseReceived)
                {
                    await _WebSocket.CloseAsync(
                        WebSocketCloseStatus.NormalClosure,
                        "Client disconnect",
                        CancellationToken.None
                    );
                    _WebSocket.Dispose();
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Disconnect error: " + e.Message);
            }
            finally
            {
                _WebSocket.Dispose();
                _WebSocket = null;
            }
        }


        public async Task Send(string action, object payload, CancellationToken cancellationToken = default)
        {
            if (_WebSocket == null || _WebSocket.State != WebSocketState.Open)
                throw new Exception("WebSocket is not connected");

            var message = new
            {
                action,
                payload
            };

            string json = JsonConvert.SerializeObject(message);
            byte[] buffer = Encoding.UTF8.GetBytes(json);

            await _WebSocket.SendAsync(
                new ArraySegment<byte>(buffer),
                WebSocketMessageType.Text,
                true,
                cancellationToken
            );
        }

        public async Task<WSResponse> SendRequest(string action, object payload, CancellationToken ct = default)
        {
            var requestId = Guid.NewGuid().ToString();

            var tcs = new TaskCompletionSource<WSResponse>();
            _pendingRequests[requestId] = new TaskCompletionSource<string>();

            var message = new
            {
                action,
                requestId,
                payload
            };

            string json = JsonConvert.SerializeObject(message);
            byte[] buffer = Encoding.UTF8.GetBytes(json);

            await _WebSocket.SendAsync(
                new ArraySegment<byte>(buffer),
                WebSocketMessageType.Text,
                true,
                ct
            );

            // Ждём строку и сразу десериализуем в WSResponse
            string responseJson = await _pendingRequests[requestId].Task;
            _pendingRequests.Remove(requestId);

            var response = JsonConvert.DeserializeObject<WSResponse>(responseJson);
            return response!;
        }


        public void On(string actionName, Action<string> callback)
        {
            if (!_handlers.TryGetValue(actionName, out var list))
            {
                list = new List<Action<string>>();
                _handlers[actionName] = list;
            }

            list.Add(callback);
        }

        public void Off(string actionName, Action<string> callback)
        {
            if (!_handlers.TryGetValue(actionName, out var list))
                return;

            list.Remove(callback);

            if (list.Count == 0)
                _handlers.Remove(actionName);
        }

        private void HandleMessage(string json)
        {
            // Десериализация в конкретный тип вместо dynamic
            var msg = JsonConvert.DeserializeObject<WSResponse>(json);

            string requestId = msg.RequestId;

            if (!string.IsNullOrEmpty(requestId) && _pendingRequests.ContainsKey(requestId))
            {
                _pendingRequests[requestId].SetResult(json);
                //_pendingRequests.Remove(requestId);
                return;
            }

            if (!string.IsNullOrEmpty(msg.Action))
            {
                HandleEvent(msg);
            }

        }

        private void HandleEvent(WSResponse msg)
        {
            if (string.IsNullOrEmpty(msg.Action))
                return;

            if (_handlers.TryGetValue(msg.Action, out var list))
            {
                foreach (var handler in list)
                {
                    handler.Invoke(msg.Data?.ToString());
                }
            }
        }
    }


    public class WSResponse
    {
        [JsonProperty("action")]
        public string? Action { get; set; }

        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("message")]
        public string? Message { get; set; }

        [JsonProperty("data")]
        public object? Data { get; set; }

        [JsonProperty("requestId")]
        public string? RequestId { get; set; }
    }
}
