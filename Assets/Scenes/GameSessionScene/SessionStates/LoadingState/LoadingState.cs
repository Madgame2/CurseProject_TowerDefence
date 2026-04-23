using Common.Services.Global;
using Common.Services.Net.Modules;
using Common.systems.SceneStates.States;
using Common.systems.SceneStates.States.Attributes;
using Common.systems.UI;
using Scenes.Session;
using System;
using UnityEngine;
using Zenject;

[RootState]
[LinkToScene(typeof(GameSessionScene))]
public class LoadingState : BaseState
{

    [Inject] private SessionNetInstaller netInstaller;
    [Inject] private GlobalStorage globalStorage;
    [Inject] private UIManager _uiManager;

    public override async void EnterToState()
    {
        _uiManager.TryOpen("LoadingUI", out object outVm);
        Scenes.Session.LoadingViewModel vm = (Scenes.Session.LoadingViewModel)outVm;

        vm.setNetInstaller(netInstaller);

        try
        {
            SessionServerInfo sessionInfo = globalStorage.Get<SessionServerInfo>("sessionInfo");
            await netInstaller.Install(sessionInfo);
        }
        catch(Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    public override void LeavFormState()
    {
        _uiManager.Close("LoadingUI");
    }
}
