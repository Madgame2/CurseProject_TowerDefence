using Scenes.SessionRework.Scripts.ECS_World.Services.ProjectileSpawner.Builder;

namespace Scenes.SessionRework.Scripts.ECS_World.Services.ProjectileSpawner.Interfaces
{
    public interface IProjectileSpawner
    {
        ProjectileBuilder SpawnProjectile(uint objectId, uint prefabId);
    }
}