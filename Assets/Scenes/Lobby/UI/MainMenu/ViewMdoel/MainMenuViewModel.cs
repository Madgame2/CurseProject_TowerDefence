using Common.systems.GameStates;
using Common.systems.SceneStates;
using Common.systems.UI;
using System;
using UnityEngine;

public class MainMenuViewModel
{
    private readonly GameStateMachine _gameStateMachine;
    private readonly SceneStateMachine<LobbyScene> _sceneStateMachine;
    private readonly NavController _navController;
    private readonly UIManager _uiManager;

    public event Action<bool> ChangeButtonsAvailable;

    public MainMenuViewModel(SceneStateMachine<LobbyScene> sceneStateMachine, NavController navController, UIManager uiManager, GameStateMachine gameStateMachine)
    {
        _sceneStateMachine = sceneStateMachine;

        _sceneStateMachine.onStateChanges += StateChangesHandle;
        _navController = navController;
        _uiManager = uiManager;
        _gameStateMachine = gameStateMachine;
    }

    public async void StateChangesHandle(Type oldState,  Type newState)
    {
        if(newState != typeof(MainLobbyState))
        {
            ChangeButtonsAvailable?.Invoke(false);
        }

        await _navController.WaitIfAnimating();

        ChangeButtonsAvailable?.Invoke(true);
    }


    public void GotoLobbyPage()
    {
        _sceneStateMachine.tryMoveToState(typeof(SessionManagementLobbyState));
    }

    public void GotoSettings()
    {
        _sceneStateMachine.tryMoveToState(typeof(SettingsLobbyState));
    }

    public async void Exit()
    {
        ChangeButtonsAvailable?.Invoke(false);
        bool shouldExit = await _uiManager.QuestionWindow("Exit from game?", "This action will result in your session being closed and all unsaved actions will be irretrievably lost.", DialogType.Danger);

        if (shouldExit)
        {
            _gameStateMachine.tryMoveToState(typeof(ExitState));
            return;
        }

        ChangeButtonsAvailable?.Invoke(true);
    }
}
