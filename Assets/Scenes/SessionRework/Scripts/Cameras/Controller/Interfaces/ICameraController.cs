using System;
using Scenes.SessionRework.Scripts.Cameras.StateMachine.States.Base;
using Scenes.SessionRework.Scripts.GameWorld.View;

namespace Scenes.SessionRework.Scripts.Cameras.Controller.Interfaces
{
    public interface ICameraController
    {
        CameraView ActiveCameraView { get; }
        void TryMoveToState<T>() where T: IState;
        void TryMoveToState(Type stateType);
    }
}