using Common.Events;
using Common.Services.Global;
using Common.Services.Net;
using Common.Services.Net.Modules;
using Common.systems.GameStates;
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
        private readonly GameStateMachine _gameStatemachine;
        private GlobalStorage _globalStorage;

        public event Action onTimeOut;
        public event Action onServerError;

        public event Action<Lobby.Entities.Lobby> onLobbyUpdated;

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
                if (_lobby == value) return;

                UnsubscribeFromLobbyEvents();

                _lobby = value;

                if (_lobby != null)
                {
                    SubscribeToLobbyEvents();
                }

                onLobbyUpdated?.Invoke(Lobby);
            }
        }

        public LobbyManager(NetService netService,
            NavController nav,
            MainThreadDispatcher dispatcher,
            UIManager uIManager,
            ProfileManager profileManager,
            GameStateMachine gameState,
            GlobalStorage globalStorage) 
        {
            _socket = netService._webSocketModule;
            navController = nav;
            _threadDispatcher = dispatcher;
            _uiManager = uIManager;
            _profileMannager = profileManager;
            _gameStatemachine = gameState;
            _globalStorage = globalStorage;
        }

        public async void Initialize()
        {
        }


        private void SubscribeToLobbyEvents()
        {
            _socket.On("Lobby_updates", lobbyUpdatesHandle);
        }

        private void UnsubscribeFromLobbyEvents()
        {
            _socket.Off("Lobby_updates", lobbyUpdatesHandle);
        }

        private bool IsHost(Lobby.Entities.Lobby lobby)
        {
            return lobby.Host == _profileMannager.Profile.UserId;
        }

        private void handleResponsesToJoin(string msg)
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

                case "NEW_HOST":
                    handleHostUpdate(lobbyEvent.hostID);
                    break;

                case "PLAYER_JOIND":
                    handleNewMember(lobbyEvent.profile);
                    break;

                case "PLAYER_LEFT":
                    handlerMemberleft(lobbyEvent.userId);
                    break;

                case "NEW_LOBBY":
                    Debug.Log("NEW_LOBBY");
                    HandlerNewLobby(lobbyEvent.lobby);
                    break;
            }
        }

        private void HandlerNewLobby(Entities.Lobby lobby)
        {
            this.Lobby = lobby;
        }

        private void handlerMemberleft(string userId)
        {
            this.Lobby.Users.Remove(userId);
            this.Lobby.LobbyUsers.RemoveAll(u => u.PlayerId == userId);

            onLobbyUpdated?.Invoke(Lobby);
        }

        private void handleNewMember(Player profile)
        {
            this.Lobby.Users.Add(profile.PlayerId);
            this.Lobby.LobbyUsers.Add(profile);

            onLobbyUpdated.Invoke(Lobby);
        }

        private void handleHostUpdate(string hostID)
        {
            this.Lobby.Host = hostID;
            onLobbyUpdated.Invoke(Lobby);
        }

        private void HandelUpdateState(string state)
        {
            switch (state)
            {
                case "IN_SEARCH":
                    _socket.On("sessionReady", HandleSessionReady);
                    Debug.Log("Subsrubet to: sessionReady");
                    _uiManager.TryOpen("SearchingPanel");
                    break;
                case "IDLE":
                    _socket.Off("sessionReady", HandleSessionReady);
                    _uiManager.Close("SearchingPanel");
                    break;
            }
        }

        private async Task HandleSessionReady(string obj)
        {
            SessionServerInfo sessionServerInfo = JsonConvert.DeserializeObject<SessionServerInfo>(obj);

            _globalStorage.Set<SessionServerInfo>("sessionInfo", sessionServerInfo);


            _threadDispatcher.Run(async () =>
            {
                _uiManager.Close("SearchingPanel");
                _uiManager.TryOpen("SearchingComplite");
                navController.PlayCinematicTransitionToSession();
                await Task.Delay(1000);

                _gameStatemachine.tryMoveToState(typeof(GameSessionState));
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
