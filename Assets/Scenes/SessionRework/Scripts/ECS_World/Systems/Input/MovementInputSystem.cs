using Scellecs.Morpeh;
using Scenes.SessionRework.Scripts.ECS_World.Components.Common;
using Scenes.SessionRework.Scripts.ECS_World.Components.Movement;
using Scenes.SessionRework.Scripts.ECS_World.Components.Players;
using UnityEngine;
using Zenject;

namespace Scenes.SessionRework.Scripts.ECS_World.Systems.Input
{
    public class MovementInputSystem :IFixedSystem
    {
        public World World { get; set; }
        
        private Filter _filter;
        private Stash<InputComponent> _inputStash;
        
        private readonly BaseInputActions _inputs;

        public MovementInputSystem(World world, BaseInputActions inputs)
        {
            World = world;
            _inputs = inputs;
        }
        
        public void OnAwake()
        {
            _filter = World.Filter.With<IDComponent>().With<PlayerComponent>().With<InputComponent>().Build();
            _inputStash = World.GetStash<InputComponent>();
        }
        public void OnUpdate(float deltaTime)
        {
            Vector2 lookInput = _inputs.SessionPlayer.Move.ReadValue<Vector2>();

            foreach (var entity in _filter)
            {
                ref var input = ref _inputStash.Get(entity);
                input.MoveDirection = new Vector3(
                    lookInput.x,
                    0,
                    lookInput.y
                );
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