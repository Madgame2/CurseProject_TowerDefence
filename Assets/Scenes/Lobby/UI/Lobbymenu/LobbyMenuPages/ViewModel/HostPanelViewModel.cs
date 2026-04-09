using Common.systems.SceneStates;
using System;
using UnityEngine;

public class HostPanelViewModel
{
    private readonly SceneStateMachine<LobbyScene> _sceneStateMachine;
    private readonly NavController _navController;

    public event Action<bool> ChangeButtonsAvailable;


    public HostPanelViewModel(SceneStateMachine<LobbyScene> sceneStateMachine, NavController navController)
    {
        _sceneStateMachine = sceneStateMachine;
        _navController = navController;

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
}
