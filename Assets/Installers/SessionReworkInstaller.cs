using Common.systems.GameStates;
using Common.systems.SceneStates;
using Common.systems.SceneStates.Graph;
using Scenes.SessionRework;
using Scenes.SessionRework.Scripts.Cameras;
using Scenes.SessionRework.Scripts.Network.Controllers;
using Scenes.SessionRework.Scripts.Network.Ineterfaces;
using Scenes.SessionRework.Scripts.Player;
using Scenes.SessionRework.Scripts.Services.Sync;
using Scenes.SessionRework.Scripts.Services.Sync.interfaces;
using Scenes.SessionRework.Scripts.World.Core;
using Scenes.SessionRework.Scripts.World.Core.interfaces.BackendParams;
using Scenes.SessionRework.Scripts.World.Core.Meta;
using Scenes.SessionRework.Scripts.World.Core.Orchestrator;
using Scenes.SessionRework.Scripts.World.Entities;
using Scenes.SessionRework.Scripts.World.Factories;
using Scenes.SessionRework.Scripts.World.Graph.Builder;
using Scenes.SessionRework.Scripts.World.Graph.Nodes.Factory;
using Scenes.SessionRework.Scripts.World.Interfaces;
using Scenes.SessionRework.Scripts.World.Pool;
using Scenes.SessionRework.Scripts.World.Providers;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class SessionReworkInstaller : MonoInstaller
    {
        [SerializeField] private ChunkReader ChunkReaderPrefab;
        
        public override void InstallBindings()
        {
            Container.BindInstance(ChunkReaderPrefab);
            
            Container.Bind<GraphReader>().AsSingle();
            Container.BindInterfacesAndSelfTo<SceneStateMachine<SessionReworkScene>>().AsSingle().NonLazy();
            
#if UNITY_EDITOR
            var gameStatemachine = Container.Resolve<GameStateMachine>();
            gameStatemachine.SetStartState<SessionReworkState>();

#endif
            Container.Bind<IPlayerInputHandler>().To<Scenes.SessionRework.Scripts.Player.PlayerInputHandler>().FromComponentInHierarchy().AsSingle();

            Container.Bind<MouseLookSettings>().AsSingle().WithArguments(15f, -90f, 90f);

            Container.BindInterfacesAndSelfTo<PlayerInputController>().FromNewComponentOnNewGameObject()
                .WithGameObjectName("PlayerInputController")
                .AsSingle()
                .NonLazy();

            Container.Bind<CameraController>().FromComponentInHierarchy().AsSingle();
            
            Container.BindInterfacesAndSelfTo<SyncController>().AsSingle();
            Container.BindInterfacesAndSelfTo<SyncService>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<NodeDirectory>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<NodeFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<NetGraphBuilder>().AsSingle();
            
            Container.BindInstance(ChunkReaderPrefab).WhenInjectedInto<ChunkReaderPool>();

            Container.BindInterfacesTo<BackendParamStorage>().AsSingle();
            
            Container.Bind<WorldHolder>().AsSingle();
        }
    }
}
