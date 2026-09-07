using Scellecs.Morpeh;
using Scenes.SessionRework.Scripts.ECS_World.Components.Camera;
using Scenes.SessionRework.Scripts.ECS_World.Components.Players;
using UnityEngine;

namespace Scenes.SessionRework.Scripts.ECS_World.Systems.Camera
{
    public class CameraSyncSystem: ILateSystem
    {
        public World World { get; set; }

        private Filter _targetFilter;
        private Filter _cameraFilter;

        private Stash<CameraViewComponent> _cameraViewStash;
        private Stash<RotationStateComponent>  _rotationStateStash;
        private Stash<CameraTargetComponent> _cameraTargetStash;
        private Stash<CharacterViewComponent> _characterViewStash;
        
        public CameraSyncSystem(World world)
        {
            World = world;
        }
        
        public void OnAwake()
        {
            _cameraViewStash = World.GetStash<CameraViewComponent>();
            _rotationStateStash = World.GetStash<RotationStateComponent>();
            _cameraTargetStash = World.GetStash<CameraTargetComponent>();
            _characterViewStash = World.GetStash<CharacterViewComponent>();
            
            _cameraFilter = World.Filter
                .With<RotationStateComponent>()
                .With<CameraViewComponent>()
                .Build();
            
            _targetFilter = World.Filter
                .With<CameraTargetComponent>()
                .With<CharacterViewComponent>()
                .Build();
        }
        
        public void OnUpdate(float deltaTime)
        {
            Transform cameraTransform = null;
            foreach (var camEntity in _cameraFilter)
            {
                ref readonly var camView = ref _cameraViewStash.Get(camEntity);
                cameraTransform = camView.CameraTransform;
                
                if (cameraTransform == null) return;

                foreach (var playerEntity in _targetFilter)
                {
                    ref var rotationStateComponent = ref _rotationStateStash.Get(camEntity);
                    ref var targetComponent = ref _cameraTargetStash.Get(playerEntity);
                    ref var characterViewComponent = ref _characterViewStash.Get(playerEntity);
                    
                    Quaternion cameraRotation = Quaternion.Euler(rotationStateComponent.Pitch, rotationStateComponent.Yaw, 0f);

                    var target = targetComponent.Target;
                    
                    Vector3 pivotPosition = target.position + (target.rotation * targetComponent.PivotOffset);
                    Vector3 finalCameraPosition = pivotPosition - (cameraRotation * targetComponent.TargetDistances);
                        
                    cameraTransform.rotation = cameraRotation;
                    cameraTransform.position = finalCameraPosition;
                    
                    if (characterViewComponent.SpineBone != null) 
                    {
                        characterViewComponent.SpineBone.localRotation *= Quaternion.Euler(rotationStateComponent.Pitch, 0f, 0f);
                    }
                }
                break;
            }
        }
        
        public void Dispose()
        {
            
        }
    }
}