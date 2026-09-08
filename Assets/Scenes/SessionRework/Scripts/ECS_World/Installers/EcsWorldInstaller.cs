using Zenject;
using Scellecs.Morpeh;
using Scenes.SessionRework.Scripts.ECS_World.Features;

namespace Scenes.SessionRework.Scripts.ECS_World.Installers
{
    public static class EcsWorldInstaller
    {
        public static void Install(
            Scellecs.Morpeh.World world,
            DiContainer container)
        {
            var updateGroup = world.CreateSystemsGroup();
            var fixedUpdateGroup = world.CreateSystemsGroup();
            var lateUpdateGroup = world.CreateSystemsGroup();
            
            ChunkLoadingFeature.AddFeature(world, updateGroup, container);
            SimulationFeature.AddFeature(world, fixedUpdateGroup, container);
            MovementFeature.AddFeature(world, updateGroup,container);
            NetworkFeature.AddFeature(world, fixedUpdateGroup,container);
            CameraFeature.AddFeature(world, lateUpdateGroup, container);
            SyncEcsWithUnityFeature.AddFeature(world, updateGroup, container);
            PlayersAnimationsFeature.AddFeature(world, updateGroup, container);
            
            world.AddSystemsGroup(0, fixedUpdateGroup);
            world.AddSystemsGroup(1,updateGroup);
            world.AddSystemsGroup(2, lateUpdateGroup);
        }
    }
}