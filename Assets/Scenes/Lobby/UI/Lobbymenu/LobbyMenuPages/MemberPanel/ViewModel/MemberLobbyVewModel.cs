using Common.systems.SceneStates;
using Common.systems.UI;
using System;
using UnityEngine;
using Zenject;

public class MemberLobbyVewModel
{
    private SceneStateMachine<LobbyScene> _sceneStateMachine;
    private NavController _navController;
    private UIManager _uiManager;

    public event Action<bool> ChangeButtonsAvailable;

    public MemberLobbyVewModel(SceneStateMachine<LobbyScene> sceneStateMachine, NavController navController, UIManager uIManager)
    {
        _sceneStateMachine = sceneStateMachine;
        _uiManager = uIManager;
        _navController = navController;

        _sceneStateMachine.onStateChanges += StateChangesHandle;
    }

    internal void InviteButtonHandler()
    {
        _uiManager.TryOpen("InvitePage");
    }

    internal void LeavTheLobbyHandler()
    {
        
    }

    internal void onBackHandler()
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

}
