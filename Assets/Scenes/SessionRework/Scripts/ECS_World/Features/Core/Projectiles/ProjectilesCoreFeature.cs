using Scellecs.Morpeh;
using Scenes.SessionRework.Scripts.ECS_World.Features.Core.Projectiles.Systems;
using Zenject;

namespace Scenes.SessionRework.Scripts.ECS_World.Features.Core.Projectiles
{
    public static class ProjectilesCoreFeature
    {
        public static void AddFeature(World world, SystemsGroup updateGroup, DiContainer container)
        {
            AddSystems(world, updateGroup, container);
        }

        private static void AddSystems(World world, SystemsGroup updateGroup, DiContainer container)
        {
            updateGroup.AddSystem(container.Instantiate<SpawnUnityObjectsSystem>());
            updateGroup.AddSystem(container.Instantiate<SyncWithUnitySystem>());
        }
    }
}