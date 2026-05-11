using Common.systems.GameStates;
using Common.systems.SceneStates;
using Common.systems.SceneStates.Graph;
using Common.systems.UI;
using System;
using UnityEditor.Rendering;
using UnityEngine;
using Zenject;

public class SessionSceneInstaller : MonoInstaller
{
    [SerializeField] private UIManager _uiManager;
    [SerializeField] bool _debugState;
    public override void InstallBindings()
    {
        if (_debugState)
        {
            var gameStatemachine = Container.Resolve<GameStateMachine>();
            gameStatemachine.SetStartState<GameSessionState>();
        }

        Container.Bind<IVfxService>().To<VfxService>().AsSingle();

        Container.Bind<CannonballPool>().FromComponentInHierarchy().AsSingle();

        Container.Bind<DirectorManager>().AsTransient();

        Container.Bind<NpcManager>().FromComponentInHierarchy().AsSingle();

        Container.Bind<BuildSystem>().FromComponentInHierarchy().AsSingle();
        Container.Bind<EntityManager>().FromComponentInHierarchy().AsSingle();
        Container.Bind<DecorationManager>().FromComponentInHierarchy().AsSingle();

        Container.Bind<ThirdPersonCamera>().FromComponentInHierarchy().AsSingle();

        Container.Bind<PlayerStorage>().FromComponentInHierarchy().AsSingle();
        Container.Bind<PlayersService>().AsSingle();

        Container.Bind<PlayersController>().FromComponentInHierarchy().AsSingle();
        Container.Bind<NetDispatcher>().AsSingle();

        
        Container.BindInterfacesTo<InputInitializer>().AsSingle();

        Container.Bind<MoveCommandSender>().AsTransient();
        Container.Bind<PlayerMovementController>().AsSingle(); //потом поменять на Transiate
        Container.Bind<PlayerInputHandler>().FromComponentInHierarchy().AsSingle();

        Container.Bind<ChankSystem>().FromComponentInHierarchy().AsSingle();

        Container.Bind<SceneStateMachine<GameSessionScene>>().AsSingle().NonLazy();
        Container.Bind<IInitializable>().To<SceneStateMachine<GameSessionScene>>().FromResolve();

        Container.Bind<GraphReader>().AsSingle();

        Container.Bind<UIManager>().FromComponentInHierarchy(_uiManager).AsSingle();
        Container.Bind<SessionNetInstaller>().AsTransient();


        Container.BindInterfacesTo<SessionSceneEnterPoint>().AsSingle();



    }
}
