using System;
using Cysharp.Threading.Tasks;
using Scenes.SessionRework.Scripts.Cameras.Controller.Interfaces;
using Scenes.SessionRework.Scripts.Cameras.StateMachine.Interfaces;
using Scenes.SessionRework.Scripts.Cameras.StateMachine.States.Base;
using Scenes.SessionRework.Scripts.GameWorld.View;
using UnityEngine;
using Zenject;

namespace Scenes.SessionRework.Scripts.Cameras.Controller.Implementation
{
    public class CameraController : ICameraController, IInitializable, IDisposable, ITickable
    {
        [Inject] private readonly CameraView _cameraView;
        [Inject] private readonly ICameraStateMachine _stateMachine;
        
        public CameraView ActiveCameraView => _cameraView;
        public void TryMoveToState<T>() where T : IState
        {
            if (_stateMachine.CurrentState != typeof(T))
            {
                _stateMachine.ChangeState<T>();
            }
        }

        public void TryMoveToState(Type stateType)
        {
            if (_stateMachine.CurrentState != stateType)
            {
                _stateMachine.ChangeState(stateType);
            }
        }

        public void Tick()
        {
            _stateMachine.Update(Time.deltaTime);
        }

        public void Initialize()
        {
             _stateMachine.Initialize();
        }

        public void Dispose()
        {
        }
    }
}