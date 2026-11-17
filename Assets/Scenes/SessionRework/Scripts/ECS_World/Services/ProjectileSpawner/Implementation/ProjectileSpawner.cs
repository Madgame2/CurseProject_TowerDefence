using Scellecs.Morpeh;
using Scenes.SessionRework.Scripts.ECS_World.Services.ProjectileSpawner.Builder;
using Scenes.SessionRework.Scripts.ECS_World.Services.ProjectileSpawner.Interfaces;

namespace Scenes.SessionRework.Scripts.ECS_World.Services.ProjectileSpawner.Implementation
{
    public class ProjectileSpawner : IProjectileSpawner
    {
        private World _world;

        public ProjectileSpawner(World world)
        {
            _world = world;
        }
        public ProjectileBuilder SpawnProjectile(uint objectId, uint prefabId)
        {
            return new ProjectileBuilder(_world).Begin(objectId, prefabId);
        }
    }
}