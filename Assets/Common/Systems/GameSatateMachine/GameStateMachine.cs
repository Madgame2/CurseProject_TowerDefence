using Common.systems.GameStates.Grpah;
using Common.systems.GameStates.States;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.LightTransport;
using Zenject;

namespace Common.systems.GameStates
{
    public class GameStateMachine : Zenject.IInitializable
    {
        private readonly DiContainer _container;
        protected readonly GraphReader _graphReader;
        protected BaseState _currentState;

        private Type _debugStartState;

        public GameStateMachine(GraphReader graphReader, DiContainer container)
        {
            _graphReader = graphReader;
            _container = container;
        }

        public void Initialize()
        {
#if !UNITY_EDITOR
            Type startState = _debugStartState!=null ? _debugStartState : _graphReader.RootState;
            tryMoveToState(startState);
#endif
        }

        public void SetStartState<T>()where T:BaseState
        {
            _debugStartState = typeof(T);

            BaseState stateInstance = (BaseState)_container.Instantiate(typeof(T));

            Type buffer = _currentState?.GetType();
            _currentState = stateInstance;
            _currentState.EnterToState(buffer);
        }

        public void tryMoveToState(Type stateType) 
        {
            if (!typeof(BaseState).IsAssignableFrom(stateType))
            {
                Debug.LogError($"{stateType} не является наследником BaseState!");
                return;
            }
            _currentState?.LeavFormState(stateType);

            BaseState stateInstance = (BaseState)_container.Instantiate(stateType);

            Type buffer= _currentState?.GetType();
            _currentState = stateInstance;
            _currentState.EnterToState(buffer);
        }
    }
}
