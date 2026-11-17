using Scellecs.Morpeh;
using Scenes.SessionRework.Scripts.ECS_World.Components.Common;
using Scenes.SessionRework.Scripts.ECS_World.Components.Movement;
using Scenes.SessionRework.Scripts.ECS_World.Components.Players;
using Scenes.SessionRework.Scripts.ECS_World.Features.Core.Input.Compoennts;
using Scenes.SessionRework.Scripts.ECS_World.Features.GamePlay.Default.PrimaryAction.Components;
using UnityEngine;

namespace Scenes.SessionRework.Scripts.ECS_World.Features.GamePlay.Default.PrimaryAction.Systems
{
    public class PrimaryActionsInputSystem : IFixedSystem
    {
        public World World { get; set; }
        
        private Filter _filter;
        private Stash<PrimaryActionComponent> _inputStash;
        
        private readonly BaseInputActions _inputs;

        public PrimaryActionsInputSystem(World world, BaseInputActions inputs)
        {
            World = world;
            _inputs = inputs;
        }
        
        public void OnAwake()
        {
            _inputs.Enable();
            
            _filter = World.Filter.With<IDComponent>().With<MyPlayerComponent>().With<InputComponent>().Build();
            _inputStash = World.GetStash<PrimaryActionComponent>();
        }
        
        public void OnUpdate(float deltaTime)
        {
            bool isPressed = _inputs.SessionPlayer.PrimaryAction.IsPressed();

            bool wasPressedThisFrame =
                _inputs.SessionPlayer.PrimaryAction.WasPressedThisFrame();

            bool wasReleasedThisFrame =
                _inputs.SessionPlayer.PrimaryAction.WasReleasedThisFrame();

            foreach (var entity in _filter)
            {
                ref var input = ref _inputStash.Get(entity);

                input.IsPressed = isPressed;
                input.WasPressed = wasPressedThisFrame;
                input.WasReleased = wasReleasedThisFrame;
            }
        }
        
        public void Dispose()
        {
            _inputs?.Disable();
            if (Application.isPlaying) 
            {
                _inputs?.Dispose();
            }
        }
    }
}