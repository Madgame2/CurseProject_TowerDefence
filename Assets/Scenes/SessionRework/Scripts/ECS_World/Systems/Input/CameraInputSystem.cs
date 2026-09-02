using Scellecs.Morpeh;
using Scenes.SessionRework.Scripts.ECS_World.Components.Camera;
using UnityEngine;

namespace Scenes.SessionRework.Scripts.ECS_World.Systems.Input
{
    public class CameraInputSystem: ISystem
    {
        public World World { get; set; }
        private Filter _filter;
        private Stash<LookInputComponent> _lookInputStash;
        
        private readonly BaseInputActions _inputs;

        public CameraInputSystem(World world, BaseInputActions inputs)
        {
            World = world;
            _inputs = inputs;
        }

        public void OnAwake()
        {
            _inputs.Enable();
            
            _filter = World.Filter.With<LookInputComponent>().Build();
            _lookInputStash = World.GetStash<LookInputComponent>();
        }
        
        public void OnUpdate(float deltaTime)
        {
            Vector2 lookDelta = _inputs.SessionPlayer.Look.ReadValue<Vector2>();

            foreach (var cameraEntity in _filter)
            {
                ref var inputComp = ref _lookInputStash.Get(cameraEntity);
                
                inputComp.Delta = lookDelta;
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