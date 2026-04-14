using Common.Services.Net.Modules;
using Common.Services.Net.Services;
using Common.systems.GameStates;
using Common.systems.GameStates.States;
using Common.systems.MainThread;
using System;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Common.Services.Net
{

    public enum NetState
    {
        Disconnected,
        Connecting,
        Connected,
        Suspected,
        Reconnecting,
        Resyncing,
        Ready
    }
    public class NetService : IInitializable
    {
        public HttpModule _httpModule { get; private set; }
        public  WebSocketModule _webSocketModule { get; private set; }

        public NetworkConfig _netConfig;
        public string BaseUrl = "http://localhost:3000"; // TEMP
        private readonly GameStateMachine _gameStateMachine;
        private readonly LiveConnectionService _liveConnectionService;
        private readonly MainThreadDispatcher _dispathcer;

        public event Action tryingConnectingSelf;
        public event Action<NetState> OnStateChanged;

        [Inject]
        public NetService(HttpModule httpModule, WebSocketModule webSocketModule, LiveConnectionService liveService, GameStateMachine gameStateMachine, MainThreadDispatcher dispathc)
        {
            _httpModule = httpModule;
            _webSocketModule = webSocketModule;
            _liveConnectionService = liveService;
            _gameStateMachine = gameStateMachine;
            _dispathcer = dispathc;

            _webSocketModule.setServerAdress("localhost:3000");

            _liveConnectionService.OnConnectionLost += HandleConnectionLost;
            _liveConnectionService.OnConnectionRestored += HandleConnectionRestored;
        }

        private async void HandleConnectionLost()
        {
            Debug.Log("Connection lost → starting reconnect flow");

            await _webSocketModule.Disconnect(); 

            OnStateChanged?.Invoke(NetState.Disconnected);

            _dispathcer.Run(() =>
            {
                _gameStateMachine.tryMoveToState(typeof(ConnectToServerState));
            });
        }

        private void HandleConnectionRestored()
        {
            Debug.Log("Connection restored → resync state");

            OnStateChanged?.Invoke(NetState.Reconnecting);
        }

        public void Initialize()
        {
            
        }
    }
}
