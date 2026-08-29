using Scellecs.Morpeh;
using Scenes.SessionRework.Scripts.ECS_World.Components.Geometry;
using Scenes.SessionRework.Scripts.ECS_World.Components.Players;
using Scenes.SessionRework.Scripts.ECS_World.Components.Simulation.Requests;

namespace Scenes.SessionRework.Scripts.ECS_World.Systems
{
    public class ApplyNewPlayerPosition: ISystem
    {
        public World World { get; set; }

        private Stash<PlayerComponent> _playerStash;
        private Stash<PositionComponent> _playerPositionStash;
        private Stash<SetPositionRequest> _setPositionStash;
        
        private Filter _filter;
        
        public ApplyNewPlayerPosition(World world)
        {
            World = world;
        }

        public void OnAwake()
        {
            _filter = World.Filter
                .With<PlayerComponent>()
                .With<PositionComponent>()
                .With<SetPositionRequest>()
                .Build();
            
            _playerStash = World.GetStash<PlayerComponent>();
            _playerPositionStash = World.GetStash<PositionComponent>();
            _setPositionStash = World.GetStash<SetPositionRequest>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var playerPosition = ref _playerPositionStash.Get(entity);
                ref var request = ref _setPositionStash.Get(entity);
                
                playerPosition.Position = request.NewPosition;
                
                _setPositionStash.Remove(entity);
            }
        }
        
        public void Dispose()
        {
            
        }
    }
}