using Scellecs.Morpeh;
using Scenes.SessionRework.Scripts.ECS_World.Systems;
using Zenject;

namespace Scenes.SessionRework.Scripts.ECS_World.Features
{
    public static class MovementFeature
    {
        public static void AddFeature(SystemsGroup group, DiContainer container)
        {
            group.AddSystem(container.Instantiate<TransformSyncSystem>());
        }
    }
}