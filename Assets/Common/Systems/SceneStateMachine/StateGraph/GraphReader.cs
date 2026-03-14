using Common.systems.SceneStates.Transitoins;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common.systems.SceneStates.Graph
{
    public class GraphReader
    {
        
        private StateGraph _graph = new();


        public Type Root { get => _graph.Root;  } 

        public void makeRootState(Type root)
        {
            _graph.makeRootState(root);
        }
        public void AddState(Type type)
        {
            _graph.AddState(type);
        }
        public void LinkStates(Transition rule)
        {
            _graph.MakeALink(rule.From, rule.To);
        }

        private class StateGraph
        {
            private HashSet<Type> _graphStates = new HashSet<Type>();
            private readonly Dictionary<Type, List<Type>> _graph = new();
            private Type _rootState;

            public Type Root { get => _rootState; }

            public void makeRootState(Type type)
            {
                if (!_graphStates.Contains(type))
                {
                    throw new ArgumentException($"type {type} is not a part of this sceneStateMachine");
                }
                _rootState = type;
            }
            public void AddState(Type type)
            {
                _graphStates.Add(type);
            }
            public void MakeALink(Type from, Type to)
            {
                if (!_graph.ContainsKey(from))
                {
                    if (!_graphStates.Contains(from))
                    {
                        throw new ArgumentException($"type {from} is not a part of this sceneStateMachine");
                    }
                }

                if (!_graph.ContainsKey(to))
                {
                    if (!_graphStates.Contains(to))
                    {
                        throw new ArgumentException($"type {to} is not a part of this sceneStateMachine");
                    }
                }

                _graphStates.Remove(from);
                _graphStates.Remove(to);

                _graph[from].Add(to);
            }
        }
    }
}