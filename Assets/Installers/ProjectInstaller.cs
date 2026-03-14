using Common.Services.SceneServices;
using Common.systems.GameStates;
using Common.systems.GameStates.Grpah;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class ProjectInstaller : MonoInstaller
    {

        public override void InstallBindings()
        {
            Container.Bind<SceneStateManager>().AsSingle();

            Container.Bind<GraphReader>().AsSingle().NonLazy();
            Container.Bind<IInitializable>().To<GraphReader>().FromResolve();

            Container.Bind<GameStateMachine>().AsSingle().NonLazy();
            Container.Bind<IInitializable>().To<GameStateMachine>().FromResolve();
        }
    }
}
