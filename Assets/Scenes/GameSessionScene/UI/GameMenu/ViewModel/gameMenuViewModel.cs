using Common.systems.GameStates;
using Common.systems.UI;
using System;
using UnityEngine;
using Zenject;

public class gameMenuViewModel
{

    [Inject] private UIManager _uiManager;
    [Inject] private GameStateMachine _gameStateMachine;


    internal void OnExitToDescktopHandler()
    {
        _gameStateMachine.tryMoveToState(typeof(ExitState));
    }

    internal void OnLeaveFromSessionHandler()
    {
        throw new NotImplementedException();
    }

    internal void OnResumeButtonHandler()
    {
        _uiManager.Close("GameMenu");
    }
}
