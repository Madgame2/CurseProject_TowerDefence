using Common.Services.Net;
using Common.Services.Net.Modules;
using Common.Services.SceneServices;
using Common.systems.GameStates;
using Common.systems.GameStates.Grpah;
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


        Container.Bind<WebSocketModule>().AsSingle();
            Container.Bind<HttpModule>().AsSingle();
            Container.Bind<NetService>().AsSingle().NonLazy();
            Container.Bind<IInitializable>().To<NetService>().FromResolve();



            Container.Bind<SceneStateManager>().AsSingle();

            Container.Bind<GraphReader>().AsSingle().NonLazy();
            Container.Bind<IInitializable>().To<GraphReader>().FromResolve();

            Container.Bind<GameStateMachine>().AsSingle().NonLazy();
            Container.Bind<IInitializable>().To<GameStateMachine>().FromResolve();

            Container.Bind<ScriptDirector>().AsTransient();
        }
    }
}
