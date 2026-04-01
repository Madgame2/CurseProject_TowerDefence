using Common.systems.GameStates.Grpah;
using Common.systems.GameStates.States;
using System;
using UnityEngine;
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
            Type startState = _debugStartState!=null ? _debugStartState : _graphReader.RootState;
            tryMoveToState(startState);
        }

        public void SetStartState<T>()where T:BaseState
        {
            _debugStartState = typeof(T);
        }

        public void tryMoveToState(Type stateType) 
        {
            if (!typeof(BaseState).IsAssignableFrom(stateType))
            {
                Debug.LogError($"{stateType} не является наследником BaseState!");
                return;
            }
            _currentState?.LeavFormState();

            BaseState stateInstance = (BaseState)_container.Instantiate(stateType);

            _currentState = stateInstance;
            _currentState.EnterToState();
        }
    }
}
