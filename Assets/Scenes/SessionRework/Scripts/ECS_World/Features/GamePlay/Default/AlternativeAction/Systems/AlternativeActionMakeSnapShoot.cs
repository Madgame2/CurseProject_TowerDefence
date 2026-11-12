using Scellecs.Morpeh;
using Scenes.SessionRework.Scripts.ECS_World.Components.Players;
using Scenes.SessionRework.Scripts.ECS_World.Features.Components;
using Scenes.SessionRework.Scripts.ECS_World.Features.Core.Input.Compoennts;
using Scenes.SessionRework.Scripts.ECS_World.Features.DTO.Enums;
using UnityEngine;

namespace Scenes.SessionRework.Scripts.ECS_World.Features.GamePlay.Default.AlternativeAction.Systems
{
    public class AlternativeActionMakeSnapShoot: IFixedSystem
    {
        public World World { get; set; }

        private Filter _playerFilter;
        private Filter _snapshotFilter;
        private Stash<AlternativeActionComponent> _alternativeActionStash;
        private Stash<CurrentStateSnapshotComponent> _currentSnapshotStash;
        
        public AlternativeActionMakeSnapShoot(World world)
        {
            World = world;
        }

        public void OnAwake()
        {
            _playerFilter = World.Filter.With<MyPlayerComponent>().With<AlternativeActionComponent>().Build();
            _snapshotFilter = World.Filter.With<CurrentStateSnapshotComponent>().Build();
            
            _alternativeActionStash = World.GetStash<AlternativeActionComponent>();
            _currentSnapshotStash =  World.GetStash<CurrentStateSnapshotComponent>();
        }
        
        public void OnUpdate(float deltaTime)
        {
            foreach (var player in _playerFilter)
            {
                ref var alternativeActionComponent = ref _alternativeActionStash.Get(player);

                foreach (var snapshotEntity in _snapshotFilter)
                {
                    ref var snapshotComponent = ref _currentSnapshotStash.Get(snapshotEntity);

                    if (alternativeActionComponent.IsPressed || alternativeActionComponent.WasPressed)
                    {
                        snapshotComponent.Snapshot.Actions |= ActionFlags.AlternativeAction;
                    }
                    else
                    {
                        snapshotComponent.Snapshot.Actions &= ~ActionFlags.AlternativeAction;
                    }
                }
            }
        }
        
        public void Dispose()
        {
            
        }
    }
}