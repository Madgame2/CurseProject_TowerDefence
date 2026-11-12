using Scenes.SessionRework.Scripts.Services.Replication.Events.Spawn.Payload;
using Scenes.SessionRework.Scripts.Services.Replication.Models;

namespace Scenes.SessionRework.Scripts.Services.Replication.Router.Modules.Spawn.Modules.Base
{
    public interface ISpawnModule
    {
        ObjectType ObjectType { get; }
        void Spawn(uint objectId, SpawnPayload payload);
    }
}