using Scellecs.Morpeh;
using Scenes.SessionRework.Scripts.ECS_World.Components.Movement;
using Scenes.SessionRework.Scripts.ECS_World.Components.Players;
using Scenes.SessionRework.Scripts.ECS_World.Components.Simulation.Requests;

namespace Scenes.SessionRework.Scripts.ECS_World.Systems
{
    public class ApplyNewMovementStateSystem: ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        
        private Stash<MovementStateComponent>  _stateStash;
        private Stash<SetMovementStateRequest> _setMovementStateRequestStash;
        
        public void OnAwake()
        {
            _filter = World.Filter
                .With<PlayerComponent>()
                .With<SetMovementStateRequest>()
                .With<MovementStateComponent>()
                .Build();
            
            _stateStash = World.GetStash<MovementStateComponent>();
            _setMovementStateRequestStash = World.GetStash<SetMovementStateRequest>();
        }
        
        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var stateComponent = ref _stateStash.Get(entity);
                ref var setMovementComponent= ref _setMovementStateRequestStash.Get(entity);
                
                stateComponent.MovementState = setMovementComponent.NewMovementState;
                
                _setMovementStateRequestStash.Remove(entity);
            }
        }
        
        public void Dispose()
        {
            
        }
    }
}