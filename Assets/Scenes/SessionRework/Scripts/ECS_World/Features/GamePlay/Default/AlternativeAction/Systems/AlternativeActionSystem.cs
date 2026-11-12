using System.Collections.Generic;
using Scellecs.Morpeh;
using Scenes.SessionRework.Characters.Base.Models;
using Scenes.SessionRework.Scripts.ECS_World.Components.Movement;
using Scenes.SessionRework.Scripts.ECS_World.Components.Players;
using Scenes.SessionRework.Scripts.ECS_World.Features.Core.Characters.Components;
using Scenes.SessionRework.Scripts.ECS_World.Features.Core.Input.Compoennts;
using Scenes.SessionRework.Scripts.ECS_World.Features.GamePlay.Default.AlternativeAction.Meta;
using Scenes.SessionRework.Scripts.Services.Replication.Models;

namespace Scenes.SessionRework.Scripts.ECS_World.Features.GamePlay.Default.AlternativeAction.Systems
{
    public class AlternativeActionSystem : ISystem
    {
        public World World { get; set; }
        
        private Filter _filter;
        private Stash<AlternativeActionComponent> _alternativeActionStash;
        private Stash<CharacterController.PlayerControllerComponent> _playerControllerStash;
        
        public AlternativeActionSystem(World world)
        {
            World = world;
        }
        
        public void OnAwake()
        {
            _filter = World.Filter
                .With<MyPlayerComponent>()
                .With<CharacterController.PlayerControllerComponent>()
                .With<AlternativeActionComponent>()
                .Build();
            
            _alternativeActionStash = World.GetStash<AlternativeActionComponent>();
            _playerControllerStash =  World.GetStash<CharacterController.PlayerControllerComponent>();
        }
        
        public void OnUpdate(float deltaTime)
        {
            foreach (var playerEntity in _filter)
            {
                ref var action = ref _alternativeActionStash.Get(playerEntity);
                ref var controller = ref _playerControllerStash.Get(playerEntity);

                var playerHandler = controller.Handler;
                
                if (playerHandler == null)
                    continue;

                if (action.WasPressed)
                {
                    playerHandler.HandleEvent(new CharacterGameplayEvent
                    {
                        Source = CharacterEventSource.LocalInput,
                        Kind = CharacterEventKind.Input,
                        Id = (ushort)CharacterInput.AlternativeAction,
                        EventType = PlayerGameplayEventType.InstantAction
                    });
                }
                
                if (action.WasReleased)
                {
                    playerHandler.HandleEvent(new CharacterGameplayEvent
                    {
                        Source = CharacterEventSource.LocalInput,
                        Kind = CharacterEventKind.Input,
                        Id = (ushort)CharacterInput.AlternativeAction,
                        EventType = PlayerGameplayEventType.ContinuousActionEnded
                    });
                }
            }
        }
        
        public void Dispose()
        {

        }
    }
}