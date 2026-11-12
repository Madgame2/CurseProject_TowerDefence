using Scellecs.Morpeh;
using Scenes.SessionRework.Scripts.ECS_World.Components.Common;
using Scenes.SessionRework.Scripts.ECS_World.Components.Players;
using Scenes.SessionRework.Scripts.ECS_World.Features.Components;

namespace Scenes.SessionRework.Scripts.ECS_World.Features.GamePlay.Default.FocusPlayerLookFeature.Systems
{
    public class MakePlayrLookSnapshotSystem:IFixedSystem
    {
        public World World { get; set; }

        private Filter _snapshotFilter;
        private Filter _playerFilter;
        
        private Stash<LookAtComponent> _lookAtStash;
        private Stash<CurrentStateSnapshotComponent> _currentStateSnapshotStash;
        
        public MakePlayrLookSnapshotSystem(World world)
        {
            World = world;
        }
        
        public void OnAwake()
        {
            _snapshotFilter = World.Filter.With<CurrentStateSnapshotComponent>()
                .Build();
            
            _playerFilter = World.Filter
                .With<MyPlayerComponent>()
                .With<LookAtComponent>()
                .Build();
            
            _lookAtStash = World.GetStash<LookAtComponent>();
            _currentStateSnapshotStash =  World.GetStash<CurrentStateSnapshotComponent>();
        }
        
        public void OnUpdate(float deltaTime)
        {
            foreach (var player in _playerFilter)
            {
                ref var lookAtComponent = ref _lookAtStash.Get(player);

                foreach (var snapshotEntity in _snapshotFilter)
                {
                    ref var snapShotComponent = ref _currentStateSnapshotStash.Get(snapshotEntity);

                    snapShotComponent.Snapshot.LookAtPoint = lookAtComponent.Position;
                }
            }
        }
        
        public void Dispose()
        {
            
        }
    }
}