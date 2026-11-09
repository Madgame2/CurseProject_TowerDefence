using System;
using Scenes.SessionRework.Scripts.Cameras.StateMachine.States.Base;
using UnityEngine;

namespace Scenes.SessionRework.Scripts.Cameras.StateMachine.States.Meta
{
    public abstract class CameraStateConfig: ScriptableObject
    {
        public abstract Type StateType { get; }
    }
    
    public abstract class CameraStateConfig<T>: CameraStateConfig  where T: IState
    {
        public override Type StateType { get => typeof(T); }
    }
}