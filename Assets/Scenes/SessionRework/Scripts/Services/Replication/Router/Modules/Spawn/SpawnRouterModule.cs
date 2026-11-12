using System.Collections.Generic;
using Scellecs.Morpeh;
using Scenes.SessionRework.Scripts.ECS_World.Components.Common;
using Scenes.SessionRework.Scripts.Services.Replication.Events;
using Scenes.SessionRework.Scripts.Services.Replication.Models;
using Scenes.SessionRework.Scripts.Services.Replication.Router.Modules.Spawn.Modules.Base;
using Scenes.SessionRework.Scripts.Services.Replication.Router.Modules.Spawn.Modules.Exceptions;

namespace Scenes.SessionRework.Scripts.Services.Replication.Router.Modules.Spawn
{
    public class SpawnRouterModule : IRouterModule
    {
        public EventType ProcessedType => EventType.SpawnEvent;
        private readonly Dictionary<ObjectType, ISpawnModule> _modules = new Dictionary<ObjectType, ISpawnModule>();

        
        public SpawnRouterModule(IEnumerable<ISpawnModule> modules)
        {
            foreach (var module in modules)
            {
                _modules.Add(module.ObjectType, module);
            }
        }
        
        public void ProcessEvent(ServerEvent serverEvent)
        {
            var spawnEvent = serverEvent.spawnEvent;

            if (!_modules.TryGetValue(spawnEvent.objectType, out var module))
            {
                throw new NotFoundSpawnModuleException(spawnEvent.objectType);
            }

            module.Spawn(spawnEvent.objectId, spawnEvent.payload);
        }
    }
}