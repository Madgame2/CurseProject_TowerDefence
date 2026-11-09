
using System;
using Scenes.SessionRework.Scripts.Cameras.StateMachine.States.Base;
using Scenes.SessionRework.Scripts.Cameras.StateMachine.States.Meta;

namespace Scenes.SessionRework.Scripts.Cameras.StateMachine.Interfaces
{
    public interface ICameraStateMachine
    {
        IState CurrentState { get; }

        void Initialize();

        void Update(float deltaTime);

        bool ChangeState<T>() where T : IState;
        bool ChangeState(Type stateType);
    }
}