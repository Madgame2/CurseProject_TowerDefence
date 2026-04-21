using Common.Services.Global;
using Common.Services.Net.Modules;
using Common.systems.ProfileSystem;
using System;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

public class SessionSceneEnterPoint : IInitializable
{
    [Inject] private VignetteController vignetteController;
    [Inject] private SessionNetInstaller netInstaller;
    [Inject] private GlobalStorage globalStorage;
    

    public async void Initialize()
    {
        try
        {
            vignetteController.FadeIn();

            SessionServerInfo sessionInfo = globalStorage.Get<SessionServerInfo>("sessionInfo");
            await netInstaller.Install(sessionInfo);

        }catch(Exception ex)
        {
            Debug.LogException(ex);
        }
    }
}
