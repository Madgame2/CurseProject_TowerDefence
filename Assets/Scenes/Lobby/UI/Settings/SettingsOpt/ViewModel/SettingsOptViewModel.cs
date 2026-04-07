using Common.systems.Configs;
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
    private readonly ConfigSystem _configs;


    private SettingOpt _currentOpt;
    private SettingImpViewModel _settingImpViewModel;

    public event Action<bool> ChangeButtonsAvailable;
    public event Action<bool> HasChanges;

    public SettingsOptViewModel(SceneStateMachine<LobbyScene> sceneStateMachine,
        NavController navController,
        UIManager uIManager,
        ConfigSystem config)
    {
        _sceneStateMachine = sceneStateMachine;
        _navController = navController;
        _uiManager = uIManager;
        _configs = config;

        _sceneStateMachine.onStateChanges += StateChangesHandle;

        _configs.hasChanges += ConfigChangesHandler;
    }

    private void ConfigChangesHandler()
    {
        HasChanges?.Invoke(true);
    }

    public void ConfirmChanges()
    {
        _configs.SaveData();
        HasChanges?.Invoke(false);
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
