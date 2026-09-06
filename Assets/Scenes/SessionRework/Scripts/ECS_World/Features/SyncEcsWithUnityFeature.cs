using Scellecs.Morpeh;
using Scenes.SessionRework.Scripts.ECS_World.Systems;
using Zenject;

namespace Scenes.SessionRework.Scripts.ECS_World.Features
{
    public static class SyncEcsWithUnityFeature
    {
        public static void AddFeature(World world, SystemsGroup group, DiContainer container)
        {
            group.AddSystem(container.Instantiate<PositionSyncSystem>());
            group.AddSystem(container.Instantiate<RotationSyncSystem>());
        }
    }
}