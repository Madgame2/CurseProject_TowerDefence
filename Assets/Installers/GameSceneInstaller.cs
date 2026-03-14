using Common.Services.SceneServices;
using UnityEngine;
using Zenject;

public class GameSceneInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<SceneParamsReader>().AsSingle();
    }
}
