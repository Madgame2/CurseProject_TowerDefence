using Scenes.SessionRework.Scripts.Services.Replication.Events;
using Scenes.SessionRework.Scripts.Services.Replication.Models;

namespace Scenes.SessionRework.Scripts.Services.Replication.Router.Modules
{
    public interface IRouterModule
    {
        EventType ProcessedType { get;  }
        void ProcessEvent(ServerEvent serverEvent);
    }
}