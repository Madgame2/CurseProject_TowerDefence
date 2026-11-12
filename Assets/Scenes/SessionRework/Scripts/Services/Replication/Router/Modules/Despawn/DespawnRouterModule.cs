using Scenes.SessionRework.Scripts.Services.Replication.Events;
using Scenes.SessionRework.Scripts.Services.Replication.Models;

namespace Scenes.SessionRework.Scripts.Services.Replication.Router.Modules.Despawn
{
    public class DespawnRouterModule : IRouterModule
    {
        public EventType ProcessedType => EventType.DespawnEvent;
        public void ProcessEvent(ServerEvent serverEvent)
        {
            throw new System.NotImplementedException();
        }
    }
}