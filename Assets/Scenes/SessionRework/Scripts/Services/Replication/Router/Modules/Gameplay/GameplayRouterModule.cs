using Scellecs.Morpeh;
using Scenes.SessionRework.Scripts.ECS_World.Components.Common;
using Scenes.SessionRework.Scripts.ECS_World.Components.Players;
using Scenes.SessionRework.Scripts.ECS_World.Features.Core.Characters.Components;
using Scenes.SessionRework.Scripts.Services.Replication.Events;
using Scenes.SessionRework.Scripts.Services.Replication.Events.Gameplay.Payload.Player;
using Scenes.SessionRework.Scripts.Services.Replication.Models;

namespace Scenes.SessionRework.Scripts.Services.Replication.Router.Modules.Gameplay
{
    public class GameplayRouterModule : IRouterModule
    {
        public EventType ProcessedType => EventType.GamePlay;
        
        private World _world;
        private Filter _playerFilter;
        private Stash<IDComponent> _idStash;
        private Stash<CharacterController.PlayerControllerComponent> _characterControllerStash;

        public GameplayRouterModule(World world)
        {
            _world = world;
            
            _idStash = _world.GetStash<IDComponent>();
            _characterControllerStash = _world.GetStash<CharacterController.PlayerControllerComponent>();
        }
        
        
        public void ProcessEvent(ServerEvent serverEvent)
        {
            var gameplayEvent = serverEvent.gameplayEvent;

            switch (gameplayEvent.objectType)
            {
                case ObjectType.Player:
                    ProcessPlayerEvent(gameplayEvent.objectId, gameplayEvent.payload.PlayerEvent);
                    break;
            }
        }

        private void ProcessPlayerEvent(uint objectID,PlayerGameplayEventPayload payloadPlayerEvent)
        {
            if (_playerFilter == null)
            {
                _playerFilter = _world.Filter
                    .With<PlayerComponent>()
                    .With<CharacterController.PlayerControllerComponent>()
                    .With<IDComponent>()
                    .Build();
            }

            foreach (var playerEntity in _playerFilter)
            {
                ref var idComponent = ref _idStash.Get(playerEntity);
                
                if(idComponent.NetId != objectID)
                    continue;
                
                ref var characterControllerComponent = ref _characterControllerStash.Get(playerEntity);

                var characterController = characterControllerComponent.CharacterController;

                characterController.ProcessAbylity(payloadPlayerEvent.ActionId, payloadPlayerEvent.EventType);
            }
        }
    }
}