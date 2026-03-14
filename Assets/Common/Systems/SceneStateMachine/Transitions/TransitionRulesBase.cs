using System.Collections.Generic;
using System;
using UnityEngine;
using Unity.VisualScripting;
using Common.systems.SceneStates.States;

namespace Common.systems.SceneStates.Transitoins
{
    public abstract class SceneStateTransitionRulesBase<T> where T : BaseState
    {
        private Type _from;
        private List<Type> _to;

        public SceneStateTransitionRulesBase()
        {
            _from = typeof(T);
        }

        public abstract void TransitList();

        public void canTransitTo<To>() where To : BaseState
        {
            _to.Add(typeof(To));
        }

        public Transition[] CreateTransitions()
        {
            Transition[] result = new Transition[_to.Count];

            for(int i = 0; i < _to.Count; i++)
            {
                result[i] = new Transition(_from, _to[i]);
            }

            return result;
        }
    }
}
