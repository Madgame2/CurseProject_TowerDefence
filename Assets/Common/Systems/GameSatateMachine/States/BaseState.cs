using UnityEngine;

namespace Common.systems.GameStates.States
{
    public abstract class BaseState
    {
        public void EnterToState()
        {
            OnEnterToState();
        }
        public void LeavFormState()
        {
            OnLeavFromState();
        }




        protected virtual void OnEnterToState() { }
        protected virtual void OnLeavFromState() { }

    }
}
