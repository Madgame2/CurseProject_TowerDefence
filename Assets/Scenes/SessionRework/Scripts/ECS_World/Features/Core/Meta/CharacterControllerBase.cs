using Scenes.SessionRework.Scripts.Services.Replication.Models;

namespace Scenes.SessionRework.Scripts.ECS_World.Features.Core.Meta
{
    public abstract class CharacterControllerBase
    {
        public abstract void ProcessAbylity(ushort actionId, PlayerGameplayEventType eventType);
    }
}