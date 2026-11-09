using System;
using System.Collections;
using Scenes.SessionRework.Scripts.Cameras.StateMachine.States.Meta;

namespace Scenes.SessionRework.Scripts.Cameras.StateMachine.States.Base
{
    public interface IStateFactoryModule
    {
        public CameraStateType CameraState { get; }
        public Type StateType { get; }
        IState Create(CameraStateConfig stateConfig);
    }
    
    public abstract  class StateFactoryModule<T>: IStateFactoryModule where T : IState
    {
        public abstract CameraStateType CameraState { get; }
        public Type StateType { get => typeof(T); }
        IState IStateFactoryModule.Create(CameraStateConfig stateConfig)=>this.Create(stateConfig);
        
        
        public abstract T Create(CameraStateConfig stateConfig);
    }
}