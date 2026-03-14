using System;
using UnityEngine;

namespace Common.systems.SceneStates.Transitoins
{
    public class Transition
    {
        public Type From { get; }
        public Type To { get; }

        public Transition(Type from, Type to)
        {
            this.From = from;
            this.To = to;
        }
    }
}
