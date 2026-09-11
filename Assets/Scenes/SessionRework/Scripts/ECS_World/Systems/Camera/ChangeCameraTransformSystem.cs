using Scellecs.Morpeh;
using Scenes.SessionRework.Scripts.ECS_World.Components.Camera;
using Scenes.SessionRework.Scripts.ECS_World.Components.Geometry;
using Scenes.SessionRework.Scripts.ECS_World.Components.Players;
using UnityEngine;

namespace Scenes.SessionRework.Scripts.ECS_World.Systems.Camera
{
    public class ChangeCameraTransformSystem: ISystem
    {
        public World World { get; set; }

        private Filter _targetFilter;
        private Filter _cameraFilter;

        private Stash<RotationStateComponent>  _rotationStateStash;
        private Stash<CameraTargetComponent> _cameraTargetStash;
        private Stash<PositionComponent> _positionStash;
        private Stash<RotationComponent> _rotationStash;
        public ChangeCameraTransformSystem(World world)
        {
            World = world;
        }
        
        public void OnAwake()
        {
            _rotationStateStash = World.GetStash<RotationStateComponent>();
            _cameraTargetStash = World.GetStash<CameraTargetComponent>();
            _positionStash =  World.GetStash<PositionComponent>();
            _rotationStash = World.GetStash<RotationComponent>();
            
            _cameraFilter = World.Filter
                .With<RotationStateComponent>()
                .With<CameraViewComponent>()
                .With<PositionComponent>()
                .With<RotationComponent>()
                .Build();
            
            _targetFilter = World.Filter
                .With<CameraTargetComponent>()
                .Build();
        }
        
        public void OnUpdate(float deltaTime)
        {
            foreach (var camEntity in _cameraFilter)
            {
                foreach (var playerEntity in _targetFilter)
                {
                    ref var rotationStateComponent = ref _rotationStateStash.Get(camEntity);
                    ref var targetComponent = ref _cameraTargetStash.Get(playerEntity);
                    ref var camEntityPositionComponent = ref _positionStash.Get(camEntity);
                    ref var camEntityRotationComponent = ref _rotationStash.Get(camEntity);
                    
                    
                    Quaternion cameraRotation = Quaternion.Euler(rotationStateComponent.Pitch, rotationStateComponent.Yaw, 0f);

                    var target = targetComponent.Target;
                    
                    Vector3 pivotPosition = target.position + (target.rotation * targetComponent.PivotOffset);
                    Vector3 finalCameraPosition = pivotPosition - (cameraRotation * targetComponent.TargetDistances);
                        
                    camEntityRotationComponent.Rotation = cameraRotation;
                    camEntityPositionComponent.Position = finalCameraPosition;
                }
                break;
            }
        }
        
        public void Dispose()
        {
            
        }
    }
}