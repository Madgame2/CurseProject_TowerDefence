using UnityEngine;
using System;
using System.Collections.Generic;
using Common.systems.GameStates.States;
using System.Linq;

namespace Common.systems.GameStates.Transitions
{
    public class Transitions
    {
        public Type From { get; }
        public Type[] To { get; }

        public Transitions(Type from, IEnumerable<Type> to) {
            this.From = from;
            this.To = to.ToArray();
        }
    }
}