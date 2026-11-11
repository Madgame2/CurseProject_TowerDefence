using Scellecs.Morpeh;
using Scenes.SessionRework.Scripts.Cameras.StateMachine.States.Meta;

namespace Scenes.SessionRework.Scripts.ECS_World.Features.Components
{
    public struct CameraStateComponent: IComponent
    {
        public CameraStateType CameraState;
    }
}