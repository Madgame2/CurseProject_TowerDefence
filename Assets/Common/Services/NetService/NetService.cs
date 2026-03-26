using Common.Services.Net.Modules;
using Common.systems.GameStates;
using System;
using UnityEngine;
using Zenject;

namespace Common.Services.Net
{
    public class NetService : IInitializable
    {
        internal readonly HttpModule _httpModule;
        internal readonly WebSocketModule _webSocketModule;

        public NetworkConfig _netConfig;
        public string BaseUrl = "http://localhost:3000"; // TEMP
        private readonly GameStateMachine gameStateMachine;


        public event Action tryingConnectingSelf;

        [Inject]
        public NetService(HttpModule httpModule, WebSocketModule webSocketModule)
        {
            _httpModule = httpModule;
            _webSocketModule = webSocketModule;
        }

        public void Initialize()
        {
            
        }
    }
}
