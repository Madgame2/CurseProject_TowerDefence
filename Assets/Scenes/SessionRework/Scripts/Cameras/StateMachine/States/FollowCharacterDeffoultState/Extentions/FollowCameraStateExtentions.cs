using Scenes.SessionRework.Scripts.Cameras.StateMachine.States.Base;
using Scenes.SessionRework.Scripts.Cameras.StateMachine.States.FollowCharacterDeffoultState.Factory;
using Zenject;

namespace Scenes.SessionRework.Scripts.Cameras.StateMachine.States.FollowCharacterDeffoultState.Extentions
{
    public static class FollowCameraStateExtentions
    {
        public static DiContainer RegisterFollowCameraState(this DiContainer container)
        {
            container.Bind<IStateFactoryModule>().To<FollowCharacterDeffoultStateFactory>().AsTransient();
            container.Bind<IState>().To<FollowCameraState>().AsTransient();
            
            return container;
        }
    }
}