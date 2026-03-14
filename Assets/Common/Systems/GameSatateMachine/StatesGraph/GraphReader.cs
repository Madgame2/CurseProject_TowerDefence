using Common.systems.GameStates.States;
using Common.systems.GameStates.States.Attributes;
using Common.systems.GameStates.Transitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Zenject;

namespace Common.systems.GameStates.Grpah
{
    public class GraphReader : IInitializable
    {
        private StateGraph _Grpah;

        public Type RootState {get => _Grpah.RootState; }


        public void Initialize()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var TranstitionRules = assembly.GetTypes()
                .Where(t =>
                !t.IsAbstract &&
                t.BaseType != null &&
                t.BaseType.IsGenericType &&
                t.BaseType.GetGenericTypeDefinition() == typeof(TransitionRulesBase<>)
                );


            _Grpah = new StateGraph();
            foreach (var rule in TranstitionRules)
            {
                var instance = (TransitionRulesBase<BaseState>)Activator.CreateInstance(rule);

                var transitions = instance.MakeTranssitions();

                _Grpah.Addtransit(transitions);
            }

            var rootType = assembly.GetTypes()
                .SingleOrDefault(t => t.GetCustomAttribute<RootStateAttribute>() != null);

            if (rootType != null )
            {
                _Grpah.RootState = rootType;
            }
        }


        private class StateGraph
        {
            private Dictionary<Type, Type[]> graph = new();
            private Type _root;

            public Type RootState
            {
                get { return _root; }
                set { _root = value; }
            }
            public void Addtransit(Transitions.Transitions t)
            {
                if (graph.ContainsKey(t.From))
                {
                    Debug.LogError($"state {t.From} already defined in graph");
                    return;
                }

                graph.Add(t.From, t.To);
            }
        }
    }
}
