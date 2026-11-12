using System.Collections.Generic;
using Scenes.SessionRework.Scripts.Services.Replication.Events;
using Scenes.SessionRework.Scripts.Services.Replication.Models;
using Scenes.SessionRework.Scripts.Services.Replication.Router.Exceptions;
using Scenes.SessionRework.Scripts.Services.Replication.Router.Interfaces;
using Scenes.SessionRework.Scripts.Services.Replication.Router.Modules;

namespace Scenes.SessionRework.Scripts.Services.Replication.Router.Implementation
{
    public class ReplicationRouter: IReplicationRouter
    {
        private readonly Dictionary<EventType, IRouterModule>  _routerModules = new();

        public ReplicationRouter(IEnumerable<IRouterModule> routerModules)
        {
            foreach (var module in routerModules)
            {
                var processedType = module.ProcessedType;
                _routerModules.Add(processedType, module);
            }
        }
        
        public void Route(ServerEvent serverEvent)
        {
            if (!_routerModules.TryGetValue(serverEvent.eventType, out IRouterModule routerModule))
            {
                throw new UnSupportedServerEventTypeException(serverEvent.eventType);
            }
            
            routerModule.ProcessEvent(serverEvent);
        }
    }
}