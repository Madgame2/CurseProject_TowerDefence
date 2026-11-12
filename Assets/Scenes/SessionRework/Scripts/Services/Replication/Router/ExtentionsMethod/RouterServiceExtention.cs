using Scenes.SessionRework.Scripts.Services.Replication.Router.Implementation;
using Scenes.SessionRework.Scripts.Services.Replication.Router.Interfaces;
using Scenes.SessionRework.Scripts.Services.Replication.Router.Modules;
using Scenes.SessionRework.Scripts.Services.Replication.Router.Modules.Despawn;
using Scenes.SessionRework.Scripts.Services.Replication.Router.Modules.Gameplay;
using Scenes.SessionRework.Scripts.Services.Replication.Router.Modules.Spawn;
using Scenes.SessionRework.Scripts.Services.Replication.Router.Modules.Spawn.Modules;
using Scenes.SessionRework.Scripts.Services.Replication.Router.Modules.Spawn.Modules.Base;
using Zenject;

namespace Scenes.SessionRework.Scripts.Services.Replication.Router.ExtentionsMethod
{
    public static class RouterServiceExtention
    {
        public static void AddReplicationRouterService(this DiContainer container)
        {
            container.Bind<ISpawnModule>().To<PlayerSpawnModule>().AsSingle();
            
            container.Bind<IRouterModule>().To<DespawnRouterModule>().AsSingle();
            container.Bind<IRouterModule>().To<GameplayRouterModule>().AsSingle();
            container.Bind<IRouterModule>().To<SpawnRouterModule>().AsSingle();
            container.Bind<IReplicationRouter>().To<ReplicationRouter>().AsSingle();
        }
    }
}