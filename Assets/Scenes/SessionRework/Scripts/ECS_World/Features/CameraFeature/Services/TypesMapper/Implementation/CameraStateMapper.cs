using System;
using System.Collections;
using System.Collections.Generic;
using Scenes.SessionRework.Scripts.Cameras.Controller.Implementation;
using Scenes.SessionRework.Scripts.Cameras.StateMachine.States.Base;
using Scenes.SessionRework.Scripts.Cameras.StateMachine.States.Meta;
using Scenes.SessionRework.Scripts.ECS_World.Features.Services.TypesMapper.Interfaces;

namespace Scenes.SessionRework.Scripts.ECS_World.Features.Services.TypesMapper.Implementation
{
    public class CameraStateMapper:ICameraStateMapper
    {
        private readonly Dictionary<CameraStateType, Type> _cameraControllers = new();
        
        public CameraStateMapper(IEnumerable<IStateFactoryModule> states)
        {
            foreach (var state in states)
            {
                _cameraControllers.Add(state.CameraState, state.StateType);
            }
        }
        
        public Type Map(CameraStateType cameraState)
        {
            return _cameraControllers[cameraState];
        }
    }
}