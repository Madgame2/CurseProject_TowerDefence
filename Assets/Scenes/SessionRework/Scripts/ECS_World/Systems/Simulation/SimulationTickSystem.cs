using System;
using Scellecs.Morpeh;
using Scenes.SessionRework.Scripts.ECS_World.Components.Simulation;

namespace Scenes.SessionRework.Scripts.ECS_World.Systems
{
    public class SimulationTickSystem:IFixedSystem
    {
        private World _world;
        
        private Filter _filter;
        private Stash<ClientSimulationComponent> _simulationStash;

        World IInitializer.World
        {
            get => _world;
            set => _world = value;
        }

        public SimulationTickSystem(World world)
        {
            _world = world;
        }

        void IInitializer.OnAwake()
        {
            _filter = _world.Filter
                .With<ClientSimulationComponent>()
                .Build();

            _simulationStash = _world.GetStash<ClientSimulationComponent>();
        }

        void ISystem.OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var simulation = ref _simulationStash.Get(entity);
                simulation.Tick++;
            }
        }
        
        void IDisposable.Dispose()
        {
        }
    }
}