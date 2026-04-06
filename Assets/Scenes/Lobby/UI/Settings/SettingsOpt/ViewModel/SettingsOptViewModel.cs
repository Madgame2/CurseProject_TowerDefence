using Common.systems.SceneStates;
using Common.systems.UI;
using System;
using UnityEngine;

public enum SettingOpt
{
    Graphics,
    Audio,
    Controls
}

public class SettingsOptViewModel
{
    private SceneStateMachine<LobbyScene> _sceneStateMachine;
    private readonly NavController _navController;
    private readonly UIManager _uiManager;

    private SettingOpt _currentOpt;
    private SettingImpViewModel _settingImpViewModel;

    public event Action<bool> ChangeButtonsAvailable;

    public SettingsOptViewModel(SceneStateMachine<LobbyScene> sceneStateMachine, NavController navController, UIManager uIManager)
    {
        _sceneStateMachine = sceneStateMachine;
        _navController = navController;
        _uiManager = uIManager;

        _sceneStateMachine.onStateChanges += StateChangesHandle;
    }

    public void OpenSettingsImp()
    {
        if (!_uiManager.IsOpen("SettingsImp"))
        {
            _uiManager.TryOpen("SettingsImp", out object vm);

            _settingImpViewModel = vm as SettingImpViewModel;
        }
    }

    public void onBack()
    {
        _sceneStateMachine.tryMoveToState(typeof(MainLobbyState));
    }

    public void OnGraphics()
    {
        _currentOpt = SettingOpt.Graphics;

        OpenSettingsImp();

        _settingImpViewModel.OpenGraphicPage();
    }

    public void OnAudio()
    {
        _currentOpt = SettingOpt.Audio;

        OpenSettingsImp();

        _settingImpViewModel.OpenAudioPage();
    }

    public void OnControls()
    {
        _currentOpt = SettingOpt.Controls;

        OpenSettingsImp();

        _settingImpViewModel.OpenControlsPage();
    }

    public async void StateChangesHandle(Type oldState, Type newState)
    {
        if (newState != typeof(SettingsLobbyState))
        {
            ChangeButtonsAvailable?.Invoke(false);
        }

        await _navController.WaitIfAnimating();

        ChangeButtonsAvailable?.Invoke(true);
    }
}
