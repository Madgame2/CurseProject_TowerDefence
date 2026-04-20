using Common.Events;
using Common.Services.Net;
using Common.Services.Net.Modules;
using Common.systems.MainThread;
using Common.systems.ProfileSystem;
using Common.systems.ProfileSystem.Entities;
using Common.systems.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Scenes.Lobby.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using Zenject;

namespace Scenes.Lobby
{
    public class LobbyManager : IInitializable
    {
        private NavController navController;
        private UIManager _uiManager;
        private MainThreadDispatcher _threadDispatcher;
        private readonly WebSocketModule _socket;
        private Lobby.Entities.Lobby _lobby = null;
        private readonly ProfileManager _profileMannager;

        public event Action onTimeOut;
        public event Action onServerError;

        public event Action onLobbyUpdated;

        private bool _inGameSearch = false;
        public event Action<bool> gameSearchStateChanges;

        public bool InGameSearch
        {
            set {
                _inGameSearch = value;
                gameSearchStateChanges?.Invoke(_inGameSearch);
            }
            get => _inGameSearch;
        }




        public Lobby.Entities.Lobby Lobby
        {
            get => _lobby;
            private set
            {
                if( _lobby == null && value != null) {
                    _socket.On("Lobby_updates", lobbyUpdatesHandle);
                }
                else
                {
                    _socket.Off("Lobby_updates", lobbyUpdatesHandle);
                }
                _lobby = value;

                onLobbyUpdated?.Invoke();
            }
        }

        public LobbyManager(NetService netService, NavController nav, MainThreadDispatcher dispatcher, UIManager uIManager, ProfileManager profileManager) 
        {
            _socket = netService._webSocketModule;
            navController = nav;
            _threadDispatcher = dispatcher;
            _uiManager = uIManager;
            _profileMannager = profileManager;
        }

        public async void Initialize()
        {
        }

        private void lobbyUpdatesHandle(string message)
        {
            Debug.Log(message);
            var lobbyEvent = JsonConvert.DeserializeObject<LobbyEvent>(message);

            Debug.Log(lobbyEvent.type);
            switch (lobbyEvent.type)
            {
                case "STATE_UPDATE":
                    HandelUpdateState(lobbyEvent.state);
                    break;
            }
        }

        private void HandelUpdateState(string state)
        {
            switch (state)
            {
                case "IN_SEARCH":
                    _socket.On("sessionReady", HandleSessionReady);
                    Debug.Log("Subsrubet to: sessionReady");
                    break;
            }
        }

        private async Task HandleSessionReady(string obj)
        {
            SessionServerInfo sessionServerInfo;

            try
            {
                sessionServerInfo = JsonConvert.DeserializeObject<SessionServerInfo>(obj);

                if (sessionServerInfo == null)
                    throw new Exception("SessionServerInfo is null");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to parse sessionReady payload: {ex}");
                return;
            }

            try
            {
                using var cts = new CancellationTokenSource();

                // ⏱️ таймаут (например 5 секунд)
                cts.CancelAfter(TimeSpan.FromSeconds(5));

                var headers = new Dictionary<string, string>
                        {
                            { "Authorization", $"{sessionServerInfo.passToken}" },
                            { "X-User-Id", _profileMannager.Profile.UserId },
                            { "X-Session-Id", sessionServerInfo.sessionId }
                        };

                var newSocket = await WebSocketModule.CreateConnectionTo(
                    sessionServerInfo.host,
                    sessionServerInfo.port,
                    headers,
                    cts.Token
                );

                await _socket.ReplaceSessionSocketAsync(newSocket);
            }
            catch (OperationCanceledException)
            {
                Debug.LogWarning("Connection to session server timed out");
                return;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to connect to session server: {ex}");
                return;
            }


            Debug.Log("Я ПОДКЛЮЧИЛСЯ");
            _threadDispatcher.Run(() =>
            {
                _uiManager.Close("SearchingPanel");
                _uiManager.TryOpen("SearchingComplite");
                navController.PlayCinematicTransitionToSession();
            });
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

        internal void SyncState()
        {
            throw new NotImplementedException();
        }
    }
}
