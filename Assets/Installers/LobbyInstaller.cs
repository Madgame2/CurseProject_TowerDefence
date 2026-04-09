using Common.Services.SceneServices.Scenes;
using Common.systems.GameStates;
using Common.systems.ProfileSystem;
using Common.systems.SceneStates;
using Common.systems.SceneStates.Graph;
using Common.systems.UI;
using Scenes.Lobby;
using System;
using UnityEngine;
using Zenject;

public class LobbyInstaller : MonoInstaller
{
    [SerializeField] private UIManager uIManager;
    [SerializeField] bool _debugState;

    public override void InstallBindings()
    {
        if (_debugState)
        {
            var gameStatemachine = Container.Resolve<GameStateMachine>();
            gameStatemachine.SetStartState<LobbyState>();
        }


        Container.Bind<UIManager>().FromInstance(uIManager).AsSingle();

        Container.Bind<NavController>().FromComponentInHierarchy().AsSingle();


        Container.Bind<GraphReader>().AsSingle();

        Container.Bind<SceneStateMachine<LobbyScene>>().AsSingle().NonLazy();
        Container.Bind<IInitializable>().To<SceneStateMachine<LobbyScene>>().FromResolve();

        Container.Bind<ProfileManager>().AsSingle().NonLazy();
        
        Container.Bind<LobbyManager>().AsSingle().NonLazy();
        Container.Bind<IInitializable>().To<LobbyManager>().FromResolve();

        Container.BindInterfacesTo<LobbySceneEntryPoint>().AsSingle();
    }
}
