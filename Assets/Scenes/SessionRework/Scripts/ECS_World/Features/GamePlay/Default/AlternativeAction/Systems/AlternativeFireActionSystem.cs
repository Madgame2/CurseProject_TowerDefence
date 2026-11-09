using System.Collections.Generic;
using Scellecs.Morpeh;
using Scenes.SessionRework.Scripts.ECS_World.Components.Movement;
using Scenes.SessionRework.Scripts.ECS_World.Components.Players;
using Scenes.SessionRework.Scripts.ECS_World.Features.Core.Characters.Components;
using Scenes.SessionRework.Scripts.ECS_World.Features.Core.Input.Compoennts;
using Scenes.SessionRework.Scripts.ECS_World.Features.GamePlay.Default.AlternativeAction.Meta;

namespace Scenes.SessionRework.Scripts.ECS_World.Features.GamePlay.Default.AlternativeAction.Systems
{
    public class AlternativeFireActionSystem : ISystem
    {
        public World World { get; set; }
        
        private Filter _filter;
        private Stash<AlternativeActionComponent> _alternativeActionStash;
        private Stash<CharacterController.PlayerControllerComponent> _playerControllerStash;
        
        public AlternativeFireActionSystem(World world)
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
                ref var alternativeActionComponent = ref _alternativeActionStash.Get(playerEntity);
                ref var playerControllerComponent = ref _playerControllerStash.Get(playerEntity);

                var playerController = playerControllerComponent.CharacterController;

                if (playerController is IAlternativeActiveable alternativeController)
                {
                    var actionModule = alternativeController.AlternativeActionModule;
                    
                    if(alternativeActionComponent.WasPressed)
                        actionModule.StartProcess();
                    
                    if(alternativeActionComponent.IsPressed)
                        actionModule.UpdateProcess(deltaTime);
                    
                    if(alternativeActionComponent.WasReleased)
                        actionModule.StopProcess();
                }
            }
        }
        
        public void Dispose()
        {

        }
    }
}