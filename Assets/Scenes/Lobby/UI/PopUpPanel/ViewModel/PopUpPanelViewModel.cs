using Common.Services.Net.Modules;
using Common.systems.GameStates;
using Common.systems.GameStates.States;
using Common.systems.UI;
using System;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

public class PopUpPanelViewModel
{
    [Inject] private UIManager _uiManager;
    [Inject] private WebSocketModule _socekt;
    [Inject] private GameStateMachine _gameStateMachine;

    internal void closePanel()
    {
        _uiManager.Close("PopUpPanel");
    }

    internal void EditProfile()
    {
        _uiManager.TryOpen("EditProfilePanel");
    }

    internal async void onLogOuthandler()
    {
        bool result = await _uiManager.QuestionWindow("Are you sure?", "This action will result in your session being closed. To restore it, you will have to re-enter your data",DialogType.Danger);

        if (result)
        {
            await _socekt.Disconnect();
            _gameStateMachine.tryMoveToState(typeof(ConnectToServerState));
        }
    }
}
