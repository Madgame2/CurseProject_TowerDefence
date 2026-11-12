using Scellecs.Morpeh;
using Scenes.SessionRework.Scripts.ECS_World.Components.Addition;
using Scenes.SessionRework.Scripts.ECS_World.Components.Geometry;
using Scenes.SessionRework.Scripts.ECS_World.Components.Movement;
using Scenes.SessionRework.Scripts.ECS_World.Components.Players;
using Scenes.SessionRework.Scripts.ECS_World.Components.Simulation;
using Scenes.SessionRework.Scripts.ECS_World.Features.Components;
using Scenes.SessionRework.Scripts.ECS_World.Features.DTO.Enums;

namespace Scenes.SessionRework.Scripts.ECS_World.Features.Systems
{
    public class MoveSnapshotSystem : IFixedSystem
    {
        public World World { get; set; }

        private Filter _currentSnapshotFilter;
        private Filter _playerFilter;
        private Stash<CurrentStateSnapshotComponent> _currentSnapshotStash;
        private Stash<InputComponent> _inputStash;
        private Stash<RotationComponent> _rotationStash;
        private Stash<JumpRequestComponent> _jumpRequestStash;

        public MoveSnapshotSystem(World world)
        {
            World = world;
        }

        public void OnAwake()
        {
            _playerFilter = World.Filter
                .With<MyPlayerComponent>()
                .With<InputComponent>()
                .With<RotationComponent>()
                .Build();

            _currentSnapshotFilter = World.Filter
                .With<CurrentStateSnapshotComponent>()
                .Build();

            _currentSnapshotStash = World.GetStash<CurrentStateSnapshotComponent>();
            _inputStash = World.GetStash<InputComponent>();
            _rotationStash = World.GetStash<RotationComponent>();
            _jumpRequestStash = World.GetStash<JumpRequestComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var player in _playerFilter)
            {
                ref var playerInput = ref _inputStash.Get(player);
                ref var rotationComponent = ref _rotationStash.Get(player);

                var jumpRequested = false;

                if (_jumpRequestStash.Has(player))
                {
                    jumpRequested = true;
                    _jumpRequestStash.Remove(player);
                }

                foreach (var snapshotEntity in _currentSnapshotFilter)
                {
                    ref var currentSnapshotComponent = ref _currentSnapshotStash.Get(snapshotEntity);

                    currentSnapshotComponent.Snapshot.MoveDirection = playerInput.MoveDirection;
                    currentSnapshotComponent.Snapshot.Rotation = rotationComponent.Rotation;

                    if (jumpRequested)
                        currentSnapshotComponent.Snapshot.Actions |= ActionFlags.JumpRequested;
                    else
                        currentSnapshotComponent.Snapshot.Actions &= ~ActionFlags.JumpRequested;
                }
            }
        }

        public void Dispose()
        {
        }
    }
}