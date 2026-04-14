using Common.systems.SceneStates;
using Common.systems.UI;
using Scenes.Lobby;
using System;
using UnityEngine;

public class HostPanelViewModel
{
    private readonly SceneStateMachine<LobbyScene> _sceneStateMachine;
    private readonly NavController _navController;
    private readonly UIManager _uiManager;
    private readonly LobbyManager _lobbyManager;

    public event Action<bool> ChangeButtonsAvailable;


    public HostPanelViewModel(LobbyManager lobbyManager ,SceneStateMachine<LobbyScene> sceneStateMachine, NavController navController, UIManager uIManager)
    {
        _sceneStateMachine = sceneStateMachine;
        _navController = navController;
        _uiManager = uIManager;
        _lobbyManager = lobbyManager;

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

    internal void OnCancelSearchClicked()
    {
        _lobbyManager.InGameSearch = false;
    }
}
