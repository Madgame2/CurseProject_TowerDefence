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
            
            SimulationFeature.AddFeature(world, fixedUpdateGroup, container);
            MovementFeature.AddFeature(world, updateGroup,container);
            
            world.AddSystemsGroup(0, fixedUpdateGroup);
            world.AddSystemsGroup(1,updateGroup);
        }
    }
}