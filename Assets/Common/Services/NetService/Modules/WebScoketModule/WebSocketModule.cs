using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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


        public bool IsConnected => _isConnected;

        private async Task ReceiveLoop(CancellationToken ct = default)
        {
            var buffer = new byte[8192];

            while (_WebSocket != null && _WebSocket.State == WebSocketState.Open && !ct.IsCancellationRequested)
            {
                try
                {
                    var result = await _WebSocket.ReceiveAsync(new ArraySegment<byte>(buffer), ct);

                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        Console.WriteLine("Server closed connection");
                        _isConnected = false;
                        onDisconnect?.Invoke();
                        break;
                    }

                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        var json = Encoding.UTF8.GetString(buffer, 0, result.Count);
                        HandleMessage(json); // тут вызываем твой обработчик
                    }
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Receive error: " + ex.Message);
                    break;
                }
            }
        }


        public void setServerAdress(string adress)
        {
            if (string.IsNullOrEmpty(adress)) throw new ArgumentNullException();
            _hostAddress = adress;
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

                    // Запускаем receive loop, чтобы HandleMessage вызывался
                    _ = Task.Run(() => ReceiveLoop());

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
                
            }
            catch (Exception e)
            {
                Debug.LogError("Disconnect error: " + e.Message);
            }
        }


        public async Task Send(string action, object payload, CancellationToken cancellationToken = default)
        {
            if (_WebSocket == null || _WebSocket.State != WebSocketState.Open)
                throw new Exception("WebSocket is not connected");

            var message = new
            {
                action = action,
                payload = payload
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

        }

        public void Off(string actionName, Action<string> callback)
        {

        }

        private void HandleMessage(string json)
        {
            // Десериализация в конкретный тип вместо dynamic
            var msg = JsonConvert.DeserializeObject<WSResponse>(json);

            string requestId = msg.RequestId;

            if (!string.IsNullOrEmpty(requestId) && _pendingRequests.ContainsKey(requestId))
            {
                _pendingRequests[requestId].SetResult(json);
                _pendingRequests.Remove(requestId);
                return;
            }

            // Иначе обычные события (On/handlers)
        }
    }


    public class WSResponse
    {
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
