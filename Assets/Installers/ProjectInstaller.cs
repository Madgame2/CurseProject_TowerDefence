using Common.Services.Global;
using Common.Services.Net;
using Common.Services.Net.Modules;
using Common.Services.Net.Services;
using Common.Services.SceneServices;
using Common.systems.Configs;
using Common.systems.GameStates;
using Common.systems.GameStates.Grpah;
using Common.systems.MainThread;
using Common.systems.ProfileSystem;
using Common.systems.ScriptDirectorSystem;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            NetworkConfig _networkConfig = new NetworkConfig("http://localhost:3000",0);

            Container.Bind<GlobalStorage>().AsSingle();

            Container.Bind<WebSocketModule>().AsSingle();
            Container.Bind<HttpModule>().AsSingle();
            Container.BindInterfacesAndSelfTo<LiveConnectionService>().AsSingle().NonLazy();
            Container.Bind<NetService>().AsSingle().NonLazy();
            Container.Bind<IInitializable>().To<NetService>().FromResolve();

            Container.Bind<ConfigSystem>().AsSingle().NonLazy();

            Container.Bind<SceneStateManager>().AsSingle();

            Container.Bind<GraphReader>().AsSingle().NonLazy();
            Container.Bind<IInitializable>().To<GraphReader>().FromResolve();

            Container.Bind<GameStateMachine>().AsSingle().NonLazy();
            Container.Bind<IInitializable>().To<GameStateMachine>().FromResolve();

            Container.Bind<ScriptDirector>().AsTransient();

            Container.Bind<ProfileManager>().AsSingle();

            Container
                .Bind<MainThreadDispatcher>()
                .FromNewComponentOnNewGameObject()
                .WithGameObjectName("MainThreadDispatcher")
                .AsSingle()
                .NonLazy();
        }
    }
}
