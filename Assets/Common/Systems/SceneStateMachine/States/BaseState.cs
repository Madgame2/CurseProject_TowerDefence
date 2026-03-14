using UnityEngine;

namespace Common.systems.SceneStates.States
{
    public abstract class BaseState
    {
        public virtual void LeavFormState()
        {

        }
        public virtual void EnterToState() { }
    }
}
