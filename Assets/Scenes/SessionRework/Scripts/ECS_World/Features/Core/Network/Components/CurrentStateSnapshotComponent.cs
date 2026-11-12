using Scellecs.Morpeh;
using Scenes.SessionRework.Scripts.ECS_World.Components.Addition;

namespace Scenes.SessionRework.Scripts.ECS_World.Features.Components
{
    public struct CurrentStateSnapshotComponent : IComponent
    {
        public PlayerStateSnapshot Snapshot;
    }
}