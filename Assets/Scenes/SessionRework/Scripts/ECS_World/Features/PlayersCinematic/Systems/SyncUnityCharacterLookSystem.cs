using Scellecs.Morpeh;
using Scenes.SessionRework.Scripts.ECS_World.Components.Common;
using Scenes.SessionRework.Scripts.ECS_World.Components.Players;
using Scenes.SessionRework.Scripts.Player.View;

namespace Scenes.SessionRework.Scripts.ECS_World.Features.PlayersCinematic.Systems
{
    public class SyncUnityCharacterLookSystem: ISystem
    {
        public World World { get; set; }

        private Filter _filter;

        private Stash<PlayerComponent> _playerStash;
        private Stash<LookAtComponent> _lookAtStash;
        
        public SyncUnityCharacterLookSystem(World world)
        {
            World = world;
        }

        public void OnAwake()
        {
            _filter = World.Filter
                .With<PlayerComponent>()
                .With<LookAtComponent>()
                .Build();
            
            _playerStash = World.GetStash<PlayerComponent>();
            _lookAtStash = World.GetStash<LookAtComponent>();
        }
        
        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var loocAtComponent = ref _lookAtStash.Get(entity);
                ref var playerComponent = ref _playerStash.Get(entity);

                var playerView = playerComponent.PlayerView;
                if(playerView == null)
                    continue;

                playerView.PlayerLooTarget.position = loocAtComponent.Position;
            }
        }
        
        public void Dispose()
        {
            
        }
    }
}