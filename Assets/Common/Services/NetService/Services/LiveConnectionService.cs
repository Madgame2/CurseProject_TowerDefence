using Common.Services.Net.Modules;
using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;


namespace Common.Services.Net.Services
{
    public class LiveConnectionService : IDisposable
    {
        private readonly WebSocketModule _socket;

        private readonly TimeSpan _timeout = TimeSpan.FromSeconds(40);
        private readonly TimeSpan _pingInterval = TimeSpan.FromSeconds(20);

        private DateTime _lastPongTime;
        private CancellationTokenSource _cts;

        private Task _pingTask;
        private Task _monitorTask;

        public event Action OnConnectionLost;
        public event Action OnConnectionRestored;

        private bool _isActive;

        private enum ConnectionState
        {
            Connected,
            Suspected,
            Disconnected
        }

        private ConnectionState _state = ConnectionState.Disconnected;

        public void ResetHeartbeat()
        {
            _lastPongTime = DateTime.UtcNow;
            _state = ConnectionState.Connected;
        }

        public LiveConnectionService(WebSocketModule net)
        {
            _socket = net;

            _socket.On("pong", OnPongReceived);

            _socket.onConnect += StartHeartBeat;
            _socket.onDisconnect += Stop;
        }

        public async void Dispose()
        {
            Stop();

            try
            {
                if (_pingTask != null)
                    await _pingTask;

                if (_monitorTask != null)
                    await _monitorTask;
            }
            catch { }

            _socket?.Disconnect();
        }

        private void StartHeartBeat()
        {
            if (_isActive)
                return;

            if (_cts != null && !_cts.IsCancellationRequested)
                return;

            _isActive = true;

            Stop();

            _cts = new CancellationTokenSource();
            _lastPongTime = DateTime.UtcNow;

            _state = ConnectionState.Connected;

            _pingTask = PingLoop();
            _monitorTask = MonitorLoop();
        }

        public void Stop()
        {
            _isActive = false;

            _cts?.Cancel();
        }

        private async Task PingLoop()
        {
            while (_cts != null && !_cts.IsCancellationRequested)
            {
                try
                {
                    _cts.Token.ThrowIfCancellationRequested();
                    await _socket.Send("ping", null);
                    Debug.Log("PING");
                }
                catch
                {
                    // если отправка упала — считаем соединение подозрительным
                    SetSuspected();
                }

                await Task.Delay(_pingInterval, _cts.Token);
            }
        }

        private void OnPongReceived(string data)
        {
            _lastPongTime = DateTime.UtcNow;
            Debug.Log("PONG");
            if (_state != ConnectionState.Connected)
            {
                _state = ConnectionState.Connected;
                OnConnectionRestored?.Invoke();
            }
        }

        private async Task MonitorLoop()
        {
            while (_cts != null && !_cts.IsCancellationRequested)
            {
                var delta = DateTime.UtcNow - _lastPongTime;

                if (delta > _timeout)
                {
                    SetSuspected();
                }

                await Task.Delay(500, _cts.Token);
            }
        }

        private void SetSuspected()
        {
            if (_state == ConnectionState.Suspected)
                return;

            if (_state == ConnectionState.Connected)
            {
                _state = ConnectionState.Suspected;
                OnConnectionLost?.Invoke();
            }
        }
    }
}
