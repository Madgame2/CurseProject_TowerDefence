using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Net.WebSockets;
using UnityEngine;

namespace Common.Services.Net.Modules
{
    public class WebSocketModule
    {
        private ClientWebSocket _WebSocket;
        private Dictionary<string, List<Action<string>>> _handlers = new();

        private string _hostAddress;
        private bool _isConnected;

        public event Action onConnect;
        public event Action onDisconnect;


        public bool IsConnected => _isConnected;

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
                    Console.WriteLine("Connected!"); // запускаем цикл получения сообщений
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


        public void On(string actionName, Action<string> callback)
        {

        }

        public void Off(string actionName, Action<string> callback)
        {

        }

        private void HandleMessage(string json)
        {

        }
    }
}
