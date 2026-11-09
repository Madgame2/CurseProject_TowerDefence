using System;
using System.Collections.Generic;
using Scenes.SessionRework.Scripts.Cameras.StateMachine.States.Base;
using Scenes.SessionRework.Scripts.Cameras.StateMachine.States.Meta;

namespace Scenes.SessionRework.Scripts.Cameras.StateMachine.States.Factory
{
    public class StateFactory: IStateFactory
    {
        Dictionary<Type, IStateFactoryModule> _modules = new Dictionary<Type, IStateFactoryModule>();
        
        public StateFactory(IEnumerable<IStateFactoryModule> stateFactoryModules)
        {
            foreach (var module in stateFactoryModules)
            {
                _modules.Add(module.StateType, module);
            }
        }
        
        public IState CreateState(CameraStateConfig StateConfig)
        {
            if (!_modules.TryGetValue(StateConfig.StateType, out var module))
            {
                throw new ArgumentException();
            }

            return module.Create(StateConfig);
        }
    }
}