using Scenes.SessionRework.Scripts.ECS_World.Services.ProjectileSpawner.Interfaces;
using Zenject;

namespace Scenes.SessionRework.Scripts.ECS_World.Services.ProjectileSpawner.ExtentionsMethods
{
    public  static class ProjectileSpawnerExtentionsMethod
    {
        public static void AddProjectileSpawnerService(this DiContainer container)
        {
            container.Bind<IProjectileSpawner>().To<Implementation.ProjectileSpawner>().AsSingle();
        }
    }
}