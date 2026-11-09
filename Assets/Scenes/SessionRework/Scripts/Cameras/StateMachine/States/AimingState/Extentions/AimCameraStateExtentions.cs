using Scenes.SessionRework.Scripts.Cameras.StateMachine.States.AimingState;
using Scenes.SessionRework.Scripts.Cameras.StateMachine.States.AimingState.Factory;
using Scenes.SessionRework.Scripts.Cameras.StateMachine.States.Base;
using Scenes.SessionRework.Scripts.Cameras.StateMachine.States.FollowCharacterDeffoultState.Factory;
using Scenes.SessionRework.Scripts.ECS_World.Features.Services.TypesMapper.Implementation;
using Scenes.SessionRework.Scripts.ECS_World.Features.Services.TypesMapper.Interfaces;
using Zenject;

namespace Scenes.SessionRework.Scripts.Cameras.StateMachine.States.FollowCharacterDeffoultState.Extentions
{
    public static class AimCameraStateExtentions
    {
        public static DiContainer RegisterAimingCameraState(this DiContainer container)
        {
            container.Bind<IStateFactoryModule>().To<AimingStateFactory>().AsTransient();
            container.Bind<IState>().To<AimingCameraState>().AsTransient();
            
            return container;
        }
    }
}