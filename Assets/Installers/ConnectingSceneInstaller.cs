using Common.Services.SceneServices;
using Common.Services.SceneServices.Scenes;
using Common.systems.SceneStates;
using Common.systems.SceneStates.Graph;
using UnityEngine;
using Zenject;

public class GameSceneInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<SceneParamsReader>().AsSingle();

        Container.Bind<GraphReader>().AsSingle();

        Container.Bind<SceneStateMachine<ConnectingToServerScene>>().AsSingle().NonLazy();
        Container.Bind<IInitializable>().To<SceneStateMachine<ConnectingToServerScene>>().FromResolve();
    }
}
