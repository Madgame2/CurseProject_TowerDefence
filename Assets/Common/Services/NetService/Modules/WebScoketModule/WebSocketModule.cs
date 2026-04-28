using Common.Services.Net.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

namespace Common.Services.Net.Modules
{
    public class WebSocketModule
    {
        private ClientWebSocket _WebSocket;
        private Dictionary<string, List<Action<string>>> _handlers = new();
        private readonly Dictionary<string, List<Func<string, Task>>> _asyncHandlers = new();
        private Dictionary<string, TaskCompletionSource<string>> _pendingRequests = new();
        [Inject] private LiveConnectionService liveService;
        private readonly SemaphoreSlim _replaceLock = new(1, 1);
        private readonly SemaphoreSlim _disconnectLock = new(1, 1);


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
            Dictionary<string, string> headers = null,
            CancellationToken ct = default)
        {
            var ws = new ClientWebSocket();

            if (headers != null)
            {
                foreach (var kv in headers)
                {
                    Debug.Log($"{kv}: {headers}");
                    ws.Options.SetRequestHeader(kv.Key, kv.Value);
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
            await _replaceLock.WaitAsync();

            ClientWebSocket oldSocket = null;
            CancellationTokenSource oldCts = null;

            try
            {
                liveService.Stop();

                oldSocket = _WebSocket;
                oldCts = _receiveCts;

                // 1. stop loop
                oldCts?.Cancel();

                // 2. wake up ReceiveAsync
                if (oldSocket != null)
                {
                    try
                    {
                        if (oldSocket.State == WebSocketState.Open ||
                            oldSocket.State == WebSocketState.CloseReceived)
                        {
                            await oldSocket.CloseOutputAsync(
                                WebSocketCloseStatus.NormalClosure,
                                "Replaced",
                                CancellationToken.None);
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.LogWarning($"Socket close error: {ex}");
                    }

                    // 🔥 HARD GUARANTEE
                    try
                    {
                        oldSocket.Abort();
                    }
                    catch { }
                }

                // 3. cleanup (NO await oldTask anymore)
                oldCts?.Dispose();

                _receiveCts = new CancellationTokenSource();
                _WebSocket = newSocket;

                _isConnected = false;

                // 4. restart
                StartReceiveLoop(_WebSocket);

                _isConnected = true;
                onConnect?.Invoke();
            }
            finally
            {
                _replaceLock.Release();
            }
        }
        private async Task ReceiveLoop(ClientWebSocket socket, CancellationToken ct)
        {
            var buffer = new byte[8192];
            var ms = new MemoryStream();

            try
            {
                while (!ct.IsCancellationRequested)
                {
                    if (socket == null || socket.State != WebSocketState.Open)
                        break;

                    ms.SetLength(0);

                    WebSocketReceiveResult result;

                    do
                    {
                        result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), ct);

                        if (ct.IsCancellationRequested)
                            return;

                        if (result.MessageType == WebSocketMessageType.Close)
                        {
                            _isConnected = false;
                            onDisconnect?.Invoke();
                            return;
                        }

                        ms.Write(buffer, 0, result.Count);

                    } while (!result.EndOfMessage);

                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        var json = Encoding.UTF8.GetString(ms.ToArray());
                        await HandleMessage(json);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // normal shutdown
            }
            catch (Exception ex)
            {
                Debug.LogError("Receive error: " + ex);
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

            _receiveTask = ReceiveLoop(socket, _receiveCts.Token);
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
            await _disconnectLock.WaitAsync();

            try
            {
                var socket = _WebSocket;
                _WebSocket = null;
                _isConnected = false;

                if (socket == null)
                    return;

                try
                {
                    if (socket.State == WebSocketState.Open ||
                        socket.State == WebSocketState.CloseReceived)
                    {
                        await socket.CloseAsync(
                            WebSocketCloseStatus.NormalClosure,
                            "Client disconnect",
                            CancellationToken.None
                        );
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError("Disconnect error: " + e);
                }
                finally
                {
                    socket.Dispose();
                }

                // остановка receive loop
                _receiveCts?.Cancel();
            }
            finally
            {
                _disconnectLock.Release();
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

        public void On(string actionName, Func<string, Task> callback)
        {
            if (!_asyncHandlers.TryGetValue(actionName, out var list))
            {
                list = new List<Func<string, Task>>();
                _asyncHandlers[actionName] = list;
            }

            list.Add(callback);
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

        public void Off(string actionName, Func<string, Task> callback)
        {
            if (!_asyncHandlers.TryGetValue(actionName, out var list))
                return;

            list.Remove(callback);

            if (list.Count == 0)
                _asyncHandlers.Remove(actionName);
        }

        private async Task HandleMessage(string json)
        {
            var jObject = JObject.Parse(json);

            // 👇 пробуем вытащить базовые поля
            var action = jObject["action"]?.ToString();
            var requestId = jObject["requestId"]?.ToString();

            // =========================
            // 1. RESPONSE (старый механизм)
            // =========================
            if (!string.IsNullOrEmpty(requestId) &&
                _pendingRequests.ContainsKey(requestId))
            {
                _pendingRequests[requestId].SetResult(json);
                return;
            }

            // =========================
            // 2. EVENT (новый или старый)
            // =========================
            if (!string.IsNullOrEmpty(action))
            {
                // 👇 пробуем старый формат
                WSResponse? msg = null;

                try
                {
                    msg = jObject.ToObject<WSResponse>();
                }
                catch { }

                // 👇 если это новый формат (без code/message и т.д.)
                if (msg == null || msg.Action == null)
                {
                    msg = new WSResponse
                    {
                        Action = action,
                        Data = jObject["data"]
                    };
                }

                await HandleEvent(msg);
                return;
            }

            Console.WriteLine("Unknown message type: " + json);
        }

        private async Task HandleEvent(WSResponse msg)
        {
            if (string.IsNullOrEmpty(msg.Action))
                return;

            var data = msg.Data?.ToString();

            // sync handlers
            if (_handlers.TryGetValue(msg.Action, out var list))
            {

                var snapshot = list.ToArray();
                foreach (var handler in snapshot)
                {
                    handler.Invoke(data);
                }
            }

            // async handlers
            if (_asyncHandlers.TryGetValue(msg.Action, out var asyncList))
            {
                var snapshot = asyncList.ToArray();

                var tasks = snapshot.Select(handler => SafeInvoke(handler, data));
                await Task.WhenAll(tasks);
            }
        }

        private async Task SafeInvoke(Func<string, Task> handler, string data)
        {
            try
            {
                await handler(data);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Handler error: {ex}");
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
