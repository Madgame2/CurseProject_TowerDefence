using Scellecs.Morpeh;
using Scenes.SessionRework.Scripts.ECS_World.Features.Components;

namespace Scenes.SessionRework.Scripts.ECS_World.Features.Systems
{
    public class ResetCurrentSnapshotSystem : IFixedSystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<CurrentStateSnapshotComponent>  _snapshotStash;
        
        public ResetCurrentSnapshotSystem(World world)
        {
            World = world;
        }

        public void OnAwake()
        {
            _filter = World.Filter.With<CurrentStateSnapshotComponent>().Build();
            
            _snapshotStash = World.GetStash<CurrentStateSnapshotComponent>();
        }
        
        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var snapshotComponent = ref _snapshotStash.Get(entity);

                snapshotComponent.Snapshot = default;
            }
        }
        
        public void Dispose()
        {
            
        }
    }
}