using UnityEngine;
using System;
using Zenject;

namespace Common.systems.GameStates.States
{
    public abstract class BaseState
    {
        [Inject] protected DiContainer _container;

        public void EnterToState(Type oldState)
        {
            OnEnterToState(oldState);
        }
        public void LeavFormState(Type newState)
        {
            OnLeavFromState(newState);
        }




        protected virtual void OnEnterToState(Type oldState) { }
        protected virtual void OnLeavFromState(Type newState) { }

    }
}
