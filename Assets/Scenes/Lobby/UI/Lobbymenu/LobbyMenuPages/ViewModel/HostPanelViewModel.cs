using Common.Services.Net;
using Common.Services.Net.Modules;
using Common.systems.SceneStates;
using Common.systems.UI;
using Scenes.Lobby;
using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class HostPanelViewModel
{
    private readonly SceneStateMachine<LobbyScene> _sceneStateMachine;
    private readonly NavController _navController;
    private readonly UIManager _uiManager;
    private readonly LobbyManager _lobbyManager;
    private readonly WebSocketModule _socket;

    public event Action<bool> ChangeButtonsAvailable;


    public HostPanelViewModel(NetService net, LobbyManager lobbyManager ,SceneStateMachine<LobbyScene> sceneStateMachine, NavController navController, UIManager uIManager)
    {
        _sceneStateMachine = sceneStateMachine;
        _navController = navController;
        _uiManager = uIManager;
        _lobbyManager = lobbyManager;
        _socket = net._webSocketModule;


        _sceneStateMachine.onStateChanges += StateChangesHandle;
    }
    internal void onBack()
    {
        _sceneStateMachine.tryMoveToState(typeof(MainLobbyState));
    }


    public async void StateChangesHandle(Type oldState, Type newState)
    {
        if (newState != typeof(SessionManagementLobbyState))
        {
            ChangeButtonsAvailable?.Invoke(false);
        }

        await _navController.WaitIfAnimating();

        ChangeButtonsAvailable?.Invoke(true);
    }

    public void onPlayButton()
    {
        _uiManager.TryOpen("HostPlay");
    }

    public void onInvite()
    {
        _uiManager.TryOpen("InvitePage");
    }

    internal void onJoinToLobby()
    {
        _uiManager.TryOpen("JoinToLobby");
    }

    internal async void OnCancelSearchClicked()
    {
        using (CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(25)))
        {
            CancellationToken token = cts.Token;

            WSResponse result = await _socket.SendRequest("CancelSearch", new { }, token);
            HandleResponses(result);
        }



        void HandleResponses(WSResponse response)
        {
            switch (response.Code){

                case 200:
                    _lobbyManager.InGameSearch = false;
                    _uiManager.Close("SearchingPanel");
                    break;

                case 500:
                default:
                    Debug.LogError("Не реализован код 500");
                    break;
            }
        }
    }
}
