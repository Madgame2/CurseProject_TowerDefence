using Common.systems.SceneStates.States.Attributes;
using Scenes.SessionRework;
using UnityEngine;

namespace Common.systems.SceneStates.States
{
    public abstract class BaseState
    {
        public virtual void LeaveFormState()
        {
        }

        public virtual void EnterToState()
        {
        }
    }
}
