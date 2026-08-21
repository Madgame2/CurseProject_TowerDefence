using Scellecs.Morpeh;
using Scenes.SessionRework.Scripts.ECS_World.Components.Simulation;
using Scenes.SessionRework.Scripts.ECS_World.Systems;
using Zenject;

namespace Scenes.SessionRework.Scripts.ECS_World.Features
{
    public static class SimulationFeature
    {
        public static void AddFeature(World world, SystemsGroup group, DiContainer container)
        {
            var simulationEntity = world.CreateEntity();

            var simulationStash = world.GetStash<ClientSimulationComponent>();
            simulationStash.Set(simulationEntity);

            group.AddSystem(container.Instantiate<SimulationTickSystem>());
        }
    }
}