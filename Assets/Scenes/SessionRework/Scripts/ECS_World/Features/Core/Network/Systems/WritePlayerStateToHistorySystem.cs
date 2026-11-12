using Scellecs.Morpeh;
using Scenes.SessionRework.Scripts.ECS_World.Components.Movement;
using Scenes.SessionRework.Scripts.ECS_World.Components.Simulation;
using Scenes.SessionRework.Scripts.ECS_World.Features.Components;
using Scenes.SessionRework.Scripts.Network.DTO;

namespace Scenes.SessionRework.Scripts.ECS_World.Features.Systems
{
    public class WritePlayerStateToHistorySystem : IFixedSystem
    {
        public World World { get; set; }
        
        private Filter _filter;
        private Filter _simulationFilter;
        private Stash<CurrentStateSnapshotComponent> _curentSnapshotStash;
        private Stash<PlayerStateHistoryComponent> _historyStash;
        private Stash<ClientSimulationComponent> _simulationStash;
        
        public WritePlayerStateToHistorySystem(World world)
        {
            World = world;
        }

        public void OnAwake()
        {
            _filter = World.Filter
                .With<CurrentStateSnapshotComponent>()
                .With<PlayerStateHistoryComponent>()
                .Build();
            
            _simulationFilter = World.Filter
                .With<ClientSimulationComponent>()
                .Build();
            
            _simulationStash = World.GetStash<ClientSimulationComponent>();
            _curentSnapshotStash = World.GetStash<CurrentStateSnapshotComponent>();
            _historyStash = World.GetStash<PlayerStateHistoryComponent>();
        }
        
        public void OnUpdate(float deltaTime)
        {
            var simulationEntity = _simulationFilter.First();

            ref var simulationComponent = ref _simulationStash.Get(simulationEntity);
            
            foreach (var entity in _filter)
            {
                ref var currentStateComponent = ref _curentSnapshotStash.Get(entity);
                ref var historyComponent = ref _historyStash.Get(entity);
                
                currentStateComponent.Snapshot.Tick = simulationComponent.Tick;
                
                historyComponent.Buffer[historyComponent.CurrentIndex] = currentStateComponent.Snapshot;

                historyComponent.CurrentIndex =
                    (historyComponent.CurrentIndex + 1) % historyComponent.Buffer.Length;
            }
        }
        
        public void Dispose()
        {
        }
    }
}