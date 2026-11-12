using Scellecs.Morpeh;
using Scenes.SessionRework.Scripts.ECS_World.Components.Addition;
using Scenes.SessionRework.Scripts.ECS_World.Components.Common;
using Scenes.SessionRework.Scripts.ECS_World.Components.Geometry;
using Scenes.SessionRework.Scripts.ECS_World.Components.Movement;
using Scenes.SessionRework.Scripts.ECS_World.Components.Simulation;
using Scenes.SessionRework.Scripts.ECS_World.Features.DTO.Enums;

namespace Scenes.SessionRework.Scripts.ECS_World.Systems.Input
{
    public class StateBufferWriteSystem: IFixedSystem
    {
        public World World { get; set; }
        
        private Filter _tickFilter;
        private Filter _playersFilter;
        
        private Stash<InputComponent> _inputStash;
        private Stash<PlayerStateHistoryComponent> _moveHistoryStash;
        private Stash<ClientSimulationComponent> _clientSimulationStash;
        private Stash<JumpRequestComponent> _jumpRequestStash;
        private Stash<RotationComponent> _rotationStash;
        private Stash<LookAtComponent> _lookAtStash;
        
        public StateBufferWriteSystem(World world)
        {
            World = world;
        }

        public void OnAwake()
        {
            _inputStash = World.GetStash<InputComponent>();
            _moveHistoryStash = World.GetStash<PlayerStateHistoryComponent>();
            _clientSimulationStash = World.GetStash<ClientSimulationComponent>();
            _jumpRequestStash = World.GetStash<JumpRequestComponent>();
            _lookAtStash = World.GetStash<LookAtComponent>();
            _rotationStash =  World.GetStash<RotationComponent>();
            
            _tickFilter = World.Filter.With<ClientSimulationComponent>().Build();
            _playersFilter = World.Filter.With<InputComponent>()
                .With<PlayerStateHistoryComponent>()
                .With<LookAtComponent>()
                .Build();
        }
        
        public void OnUpdate(float deltaTime)
        {
            foreach (var tickEntity in _tickFilter)
            {
                ref var tick = ref _clientSimulationStash.Get(tickEntity);

                foreach (var player in _playersFilter)
                {
                    ref var playerInput = ref _inputStash.Get(player);
                    ref var playerBuffer = ref _moveHistoryStash.Get(player);
                    ref var lookAtComponent = ref _lookAtStash.Get(player);
                    ref var rotationComponent = ref _rotationStash.Get(player);

                    var jumpRequested = false;
                    if (_jumpRequestStash.Has(player))
                    {
                        jumpRequested = true;
                        _jumpRequestStash.Remove(player);
                    }
                    
                    var playerSnapShoot = new PlayerStateSnapshot
                    {
                        MoveDirection = playerInput.MoveDirection,
                        Rotation = rotationComponent.Rotation,
                        LookAtPoint = lookAtComponent.Position,
                        Tick = tick.Tick,
                    };

                    if (jumpRequested)
                        playerSnapShoot.Actions |= ActionFlags.JumpRequested;


                    playerBuffer.Buffer[playerBuffer.CurrentIndex] = playerSnapShoot;

                    playerBuffer.CurrentIndex =
                        (playerBuffer.CurrentIndex + 1) % playerBuffer.Buffer.Length;
                }

                break;
            }
        }
        
        public void Dispose()
        {

        }
    }
}