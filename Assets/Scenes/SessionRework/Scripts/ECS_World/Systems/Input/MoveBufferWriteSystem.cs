using Scellecs.Morpeh;
using Scenes.SessionRework.Scripts.ECS_World.Components.Addition;
using Scenes.SessionRework.Scripts.ECS_World.Components.Movement;
using Scenes.SessionRework.Scripts.ECS_World.Components.Simulation;

namespace Scenes.SessionRework.Scripts.ECS_World.Systems.Input
{
    public class MoveBufferWriteSystem: IFixedSystem
    {
        public World World { get; set; }
        
        private Filter _tickFilter;
        private Filter _playersFilter;
        
        private Stash<InputComponent> _inputStash;
        private Stash<MoveInputHistoryComponent> _moveHistoryStash;
        private Stash<ClientSimulationComponent> _clientSimulationStash;
        private Stash<JumpRequestComponent> _jumpRequestStash;
        
        public MoveBufferWriteSystem(World world)
        {
            World = world;
        }

        public void OnAwake()
        {
            _inputStash = World.GetStash<InputComponent>();
            _moveHistoryStash = World.GetStash<MoveInputHistoryComponent>();
            _clientSimulationStash = World.GetStash<ClientSimulationComponent>();
            _jumpRequestStash = World.GetStash<JumpRequestComponent>();
            
            _tickFilter = World.Filter.With<ClientSimulationComponent>().Build();
            _playersFilter = World.Filter.With<InputComponent>()
                .With<MoveInputHistoryComponent>()
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

                    var jumpRequested = false;
                    if (_jumpRequestStash.Has(player))
                    {
                        jumpRequested = true;
                        _jumpRequestStash.Remove(player);
                    }
                    
                    playerBuffer.Buffer[playerBuffer.CurrentIndex] = new MoveInputCommand
                    {
                        MoveDirection = playerInput.MoveDirection,
                        Tick = tick.Tick,
                        JumpRequested = jumpRequested,
                    };

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