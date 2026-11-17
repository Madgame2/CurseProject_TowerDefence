using Scellecs.Morpeh;
using Scenes.SessionRework.Scripts.ECS_World.Components.Players;
using Scenes.SessionRework.Scripts.ECS_World.Features.Components;
using Scenes.SessionRework.Scripts.ECS_World.Features.Core.Input.Compoennts;
using Scenes.SessionRework.Scripts.ECS_World.Features.DTO.Enums;
using Scenes.SessionRework.Scripts.ECS_World.Features.GamePlay.Default.PrimaryAction.Components;

namespace Scenes.SessionRework.Scripts.ECS_World.Features.GamePlay.Default.PrimaryAction.Systems
{
    public class PrimaryActionMakeSnapshotSystem : IFixedSystem
    {
        public World World { get; set; }

        private Filter _playerFilter;
        private Filter _snapshotFilter;
        private Stash<PrimaryActionComponent> _primaryActionStash;
        private Stash<CurrentStateSnapshotComponent> _currentSnapshotStash;
        
        public PrimaryActionMakeSnapshotSystem(World world)
        {
            World = world;
        }

        public void OnAwake()
        {
            _playerFilter = World.Filter.With<MyPlayerComponent>().With<PrimaryActionComponent>().Build();
            _snapshotFilter = World.Filter.With<CurrentStateSnapshotComponent>().Build();
            
            _primaryActionStash = World.GetStash<PrimaryActionComponent>();
            _currentSnapshotStash =  World.GetStash<CurrentStateSnapshotComponent>();
        }
        
        public void OnUpdate(float deltaTime)
        {
            foreach (var player in _playerFilter)
            {
                ref var alternativeActionComponent = ref _primaryActionStash.Get(player);

                foreach (var snapshotEntity in _snapshotFilter)
                {
                    ref var snapshotComponent = ref _currentSnapshotStash.Get(snapshotEntity);

                    if (alternativeActionComponent.IsPressed || alternativeActionComponent.WasPressed)
                    {
                        snapshotComponent.Snapshot.Actions |= ActionFlags.PrimaryAction;
                    }
                    else
                    {
                        snapshotComponent.Snapshot.Actions &= ~ActionFlags.PrimaryAction;
                    }
                }
            }
        }
        
        public void Dispose()
        {
            
        }
    }
}