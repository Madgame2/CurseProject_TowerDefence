using Scenes.SessionRework.Scripts.Cameras.StateMachine.States.Meta;

namespace Scenes.SessionRework.Scripts.Cameras.StateMachine.States.Base
{
    public interface IState : ICameraStateLifecycle
    {
        static CameraStateType CameraState { get; }
        void Update(float deltaTime);
    }
}