using Scenes.SessionRework.Scripts.Cameras.StateMachine.States.Meta;

namespace Scenes.SessionRework.Scripts.Cameras.StateMachine.States.Base
{
    public interface IStateFactory
    {
        IState CreateState(CameraStateConfig machineConfigInitialState);
    }
}