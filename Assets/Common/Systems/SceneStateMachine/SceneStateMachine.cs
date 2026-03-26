using Common.Services.SceneServices.Scenes;
using Common.systems.SceneStates.Graph;
using Common.systems.SceneStates.States;
using Common.systems.SceneStates.States.Attributes;
using Common.systems.SceneStates.Transitoins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Zenject;

namespace Common.systems.SceneStates
{
    public class SceneStateMachine<T> : IInitializable where T : SceneBase 
    {
        private readonly DiContainer _container;
        private readonly GraphReader _graphReader;
        private readonly Type _sceneType;

        private BaseState _currentState = null;

        public SceneStateMachine(GraphReader graphReader, DiContainer container)
        {
            this._graphReader = graphReader;
            this._container = container;
            this._sceneType = typeof(T);


            Assembly assembly = Assembly.GetExecutingAssembly();
            IEnumerable<Type> states = assembly.GetTypes()
                .Where(t=>
                !t.IsAbstract&&
                typeof(BaseState).IsAssignableFrom(t)&&
                t.GetCustomAttribute<LinkToSceneAttribute>()?.SceneType == _sceneType).ToArray();

            foreach (Type t in states)
            {
                _graphReader.AddState(t);
            }

            var root = states.Where(t => t.GetCustomAttribute<RootStateAttribute>() != null);

            if (root.Count() <= 0)
            {
                Debug.LogError($"No root State in {_sceneType}");
            }
            else if (root.Count() > 1)
            {
                Debug.LogError($"too many root State in {_sceneType}");
            }
            else
            {
                _graphReader.makeRootState(root.First());
            }
            


            var TransitionRules = assembly.GetTypes()
                                    .Where(t =>
                                    !t.IsAbstract &&
                                    t.BaseType != null &&
                                    t.BaseType.IsGenericType &&
                                    t.BaseType.GetGenericTypeDefinition() == typeof(SceneStateTransitionRulesBase<>)&&
                                    t.GetCustomAttribute<LinkToSceneAttribute>()?.SceneType == _sceneType
                                    );

            foreach (var rule in TransitionRules)
            {
                var instance = (SceneStateTransitionRulesBase<BaseState>)Activator.CreateInstance(rule);

                var transitions = instance.CreateTransitions();
                foreach (var transition in transitions)
                {
                    _graphReader.LinkStates(transition);
                }
            }
        }

        public void Initialize()
        {
            tryMoveToState(_graphReader.Root);

        }

        public bool tryMoveToState(Type stateType) 
        {
            if (!typeof(BaseState).IsAssignableFrom(stateType))
            {
                Debug.LogError($"{stateType} не является наследником BaseState!");
                return false;
            }
            _currentState?.LeavFormState();

            BaseState stateInstance = (BaseState)_container.Instantiate(stateType);

            _currentState = stateInstance;
            _currentState.EnterToState();

            return true;
        }
    }
}
