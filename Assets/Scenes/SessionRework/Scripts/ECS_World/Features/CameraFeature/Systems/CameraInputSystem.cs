using Scellecs.Morpeh;
using Scenes.SessionRework.Scripts.Cameras.Services;
using Scenes.SessionRework.Scripts.Cameras.StateMachine.States.Meta;
using Scenes.SessionRework.Scripts.ECS_World.Components.Camera;
using Scenes.SessionRework.Scripts.ECS_World.Features.Components;
using UnityEngine;

namespace Scenes.SessionRework.Scripts.ECS_World.Systems.Input
{
    public class CameraInputSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<LookInputComponent> _lookInputStash;
        private Stash<CameraStateComponent> _cameraStateStash;
        private Stash<SetCameraStateRequest> _setCameraStateStash;

        private readonly BaseInputActions _inputs;
        private readonly CameraInputService _cameraInput;
        
        public CameraInputSystem(
            World world,
            BaseInputActions inputs,
            CameraInputService cameraInput)
        {
            World = world;
            _inputs = inputs;
            _cameraInput = cameraInput;
        }

        public void OnAwake()
        {
            _inputs.Enable();

            _filter = World.Filter
                .With<LookInputComponent>()
                .With<CameraStateComponent>()
                .Build();

            _lookInputStash = World.GetStash<LookInputComponent>();
            _cameraStateStash = World.GetStash<CameraStateComponent>();
            _setCameraStateStash = World.GetStash<SetCameraStateRequest>();
        }

        public void OnUpdate(float deltaTime)
        {
            ProcessLookDelta();
           // ProcessAimRequest();
        }

        private void ProcessAimRequest()
        {
            var rightClick = _inputs.SessionPlayer.RightClick;

            var cameraEntity = _filter.First();
            ref var cameraStateComponent = ref _cameraStateStash.Get(cameraEntity);
            
            if (rightClick.WasPressedThisFrame())
            {
                if (cameraStateComponent.CameraState == CameraStateType.Default)
                {
                    _setCameraStateStash.Set(cameraEntity, new SetCameraStateRequest{NewCameraState = CameraStateType.Aiming});
                }
            }

            if (rightClick.WasReleasedThisFrame())
            {
                if (cameraStateComponent.CameraState == CameraStateType.Aiming)
                {
                    _setCameraStateStash.Set(cameraEntity, new SetCameraStateRequest{NewCameraState = CameraStateType.Default});
                }
            }
        }

        private void ProcessLookDelta()
        {
            Vector2 lookDelta =
                _inputs.SessionPlayer.Look.ReadValue<Vector2>();

            _cameraInput.SetLookDelta(lookDelta);

            foreach (var entity in _filter)
            {
                ref var input = ref _lookInputStash.Get(entity);
                input.Delta = lookDelta;
            }
        }

        public void Dispose()
        {
            _inputs?.Disable();
        }
    }
}