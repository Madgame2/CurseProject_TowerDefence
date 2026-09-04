using Scellecs.Morpeh;
using Scenes.SessionRework.Scripts.ECS_World.Components.Camera;
using Scenes.SessionRework.Scripts.ECS_World.Components.Players;
using UnityEngine;

namespace Scenes.SessionRework.Scripts.ECS_World.Systems.Camera
{
    public class PlayerLookSystem : ISystem
    {
        public World World { get; set; }

        private Filter _cameraFilter;
        private Filter _characterFilter;

        private Stash<LookInputComponent> _lookInputStash;
        private Stash<RotationStateComponent> _rotationStateComponent;
        private Stash<CharacterViewComponent> _characterViewStash;
        private Stash<CameraRestrictionsComponent> _cameraRestrictionsStash;

        public PlayerLookSystem(World world)
        {
            World = world;
        }

        public void OnAwake()
        {
            _lookInputStash = World.GetStash<LookInputComponent>();
            _rotationStateComponent = World.GetStash<RotationStateComponent>();
            _characterViewStash = World.GetStash<CharacterViewComponent>();
            _cameraRestrictionsStash = World.GetStash<CameraRestrictionsComponent>();

            _cameraFilter = World.Filter
                .With<LookInputComponent>()
                .With<RotationStateComponent>()
                .With<CameraRestrictionsComponent>()
                .Build();

            _characterFilter = World.Filter
                .With<MyPlayerComponent>()
                .With<CharacterViewComponent>()
                .Build();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var cameraEntity in _cameraFilter)
            {
                ref var lookInputComponent = ref _lookInputStash.Get(cameraEntity);
                ref var rotationStateComponent = ref _rotationStateComponent.Get(cameraEntity);
                ref var cameraRestrictionsComponent = ref _cameraRestrictionsStash.Get(cameraEntity);
                
                var sensitivity = cameraRestrictionsComponent.Sensitivity;

                rotationStateComponent.Yaw += lookInputComponent.Delta.x * sensitivity;
                rotationStateComponent.Pitch -= lookInputComponent.Delta.y * sensitivity;

                rotationStateComponent.Yaw %= 360;

                var maxPitch = cameraRestrictionsComponent.MaxPitch;
                var minPitch = cameraRestrictionsComponent.MinPitch;

                rotationStateComponent.Pitch = Mathf.Clamp(rotationStateComponent.Pitch, minPitch, maxPitch);

                foreach (var characterEntity in _characterFilter)
                {
                    ref var characterView = ref _characterViewStash.Get(characterEntity);
                    if (characterView.CharacterRoot != null)
                    {
                        characterView.CharacterRoot.rotation = Quaternion.Euler(0f, rotationStateComponent.Yaw, 0f);
                    }
                }
            }
        }

        public void Dispose()
        {
        }
    }
}