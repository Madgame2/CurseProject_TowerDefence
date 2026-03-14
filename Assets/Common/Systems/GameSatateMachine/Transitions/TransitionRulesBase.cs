using Common.systems.GameStates.States;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace Common.systems.GameStates.Transitions
{
    public abstract class TransitionRulesBase<T> where T : BaseState
    {
        private Type _From;
        private List<Type> _To;

        public TransitionRulesBase()
        {
            _From = typeof(T);
        }

        public void CanTrasitTo<To>() where To : BaseState
        {
            _To.Add(typeof(To));
            _To = new List<Type>();
        }

        public abstract void TransistionList();


        public Transitions MakeTranssitions()
        {
            TransistionList();

            return new Transitions(_From, _To);
        }
    }
}
