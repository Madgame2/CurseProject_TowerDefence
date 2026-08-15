using Common.systems.GameStates;
using Common.systems.SceneStates;
using Common.systems.SceneStates.Graph;
using Scenes.SessionRework;
using Scenes.SessionRework.Scripts.Cameras;
using Scenes.SessionRework.Scripts.Network.Controllers;
using Scenes.SessionRework.Scripts.Player;
using Scenes.SessionRework.Scripts.Services.Sync;
using Scenes.SessionRework.Scripts.World.Core;
using Scenes.SessionRework.Scripts.World.Core.Meta;
using Scenes.SessionRework.Scripts.World.Entities;
using Scenes.SessionRework.Scripts.World.Entities.Chunk;
using Scenes.SessionRework.Scripts.World.Graph.Builder;
using Scenes.SessionRework.Scripts.World.Graph.Nodes.Factory;
using Scenes.SessionRework.Scripts.World.Pool;
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
            
            Container.BindInterfacesAndSelfTo<DecorationGraphBuilder>().AsTransient();
        }
    }
}
