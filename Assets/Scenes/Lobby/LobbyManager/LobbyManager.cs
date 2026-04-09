using Common.Services.Net;
using Common.Services.Net.Modules;
using Common.systems.ProfileSystem.Entities;
using Newtonsoft.Json.Linq;
using Scenes.Lobby.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Scenes.Lobby
{
    public class LobbyManager : IInitializable
    {
        private readonly WebSocketModule _socket;
        private Lobby.Entities.Lobby _lobby = null;

        public event Action onTimeOut;
        public event Action onServerError;

        public event Action onLobbyUpdated;

        public Lobby.Entities.Lobby Lobby
        {
            get => _lobby;
            private set
            {
                _lobby = value;
                onLobbyUpdated?.Invoke();
            }
        }

        public LobbyManager(NetService netService) 
        {
            _socket = netService._webSocketModule;

        }

        public async void Initialize()
        {
        }

        public async Task Init()
        {
            using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(25))) {
                CancellationToken token = cts.Token;

                try
                {
                    var response = await _socket.SendRequest("CreateLobby", new { }, token);

                    Lobby.Entities.Lobby result = await hanldeResponce(response);

                    Debug.Log(result?.Id);
                    Lobby = result;
                }
                catch (OperationCanceledException)
                {
                    onTimeOut?.Invoke();
                }
            }

            async Task<Lobby.Entities.Lobby> hanldeResponce(WSResponse response)
            {
                switch (response.Code)
                {
                    case 200:
                        {
                            Lobby.Entities.Lobby res = (response.Data as JObject)?.ToObject<Lobby.Entities.Lobby>();
                            return res;
                        }
                    case 403:
                        {
                            return await tryConnectToExistLobby();
                        }
                    case 500:
                    default:
                        {
                            onServerError?.Invoke();
                        }
                        break;
                }
                return null;
            }
        }



        private async Task<Lobby.Entities.Lobby> tryConnectToExistLobby()
        {
            using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(25))) {
                CancellationToken token = cts.Token;

                try
                {
                    var responce = await _socket.SendRequest("GetMyLobby", new { });

                    var result = await hanldeResponce(responce);

                    return result;
                }
                catch (OperationCanceledException)
                {
                    onTimeOut?.Invoke();
                }
                return null;
            }

            async Task<Lobby.Entities.Lobby> hanldeResponce(WSResponse response)
            {
                switch (response.Code)
                {
                    case 200:
                        {
                            Lobby.Entities.Lobby res = (response.Data as JObject)?.ToObject<Lobby.Entities.Lobby>();
                            return res;
                        }
                    case 404:
                    case 500:
                    default:
                        {
                            onServerError?.Invoke();
                        }
                        break;
                }

                return null;
            }
        }
    }
}
