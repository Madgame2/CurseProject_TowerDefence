using Scellecs.Morpeh;
using Scenes.SessionRework.Scripts.ECS_World.Components.Common;
using Scenes.SessionRework.Scripts.ECS_World.Components.Geometry;
using Scenes.SessionRework.Scripts.ECS_World.Features.Core.Projectiles.Components;
using UnityEngine;

namespace Scenes.SessionRework.Scripts.ECS_World.Features.Core.Projectiles.Systems
{
    public class SyncWithUnitySystem:ISystem
    {
        public World World { get; set; }

        private Filter _projectilesFilter;
        private Stash<PositionComponent> _positionStash;
        private Stash<UnityViewComponent> _unityViewStash;
        private Stash<RotationComponent> _rotationStash;
        
        
        private const float PositionSmoothing = 20f;     
        private const float RotationSmoothing = 20f;
        private const float TeleportDistance = 5f;  
        
        public SyncWithUnitySystem(World world)
        {
            World = world;
        }

        public void OnAwake()
        {
            _projectilesFilter = World.Filter
                .With<ProjectileTag>()
                .With<PositionComponent>()
                .With<RotationComponent>()
                .With<UnityViewComponent>()
                .Build();
            
            _positionStash = World.GetStash<PositionComponent>();
            _unityViewStash = World.GetStash<UnityViewComponent>();
            _rotationStash =  World.GetStash<RotationComponent>();
        }
        
        public void OnUpdate(float deltaTime)
        {
            var positionT = 1f - Mathf.Exp(-PositionSmoothing * deltaTime);
            var rotationT = 1f - Mathf.Exp(-RotationSmoothing * deltaTime);

            foreach (var projectileEntity in _projectilesFilter)
            {
                ref var positionComponent = ref _positionStash.Get(projectileEntity);
                ref var unityViewComponent = ref _unityViewStash.Get(projectileEntity);
                ref var rotationComponent = ref _rotationStash.Get(projectileEntity);

                var viewTransform = unityViewComponent.GameObject.transform;

                var targetPosition = positionComponent.Position;
                var targetRotation = rotationComponent.Rotation;

                var isTeleport =
                    (targetPosition - viewTransform.position).sqrMagnitude > TeleportDistance * TeleportDistance;

                if (isTeleport)
                {
                    viewTransform.SetPositionAndRotation(targetPosition, targetRotation);
                    continue;
                }

                viewTransform.SetPositionAndRotation(
                    Vector3.Lerp(viewTransform.position, targetPosition, positionT),
                    Quaternion.Slerp(viewTransform.rotation, targetRotation, rotationT));
            }
        }
        
        public void Dispose()
        {
            
        }
    }
}