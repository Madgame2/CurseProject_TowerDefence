using Scellecs.Morpeh;
using Scenes.SessionRework.Scripts.ECS_World.Components.Common;
using Scenes.SessionRework.Scripts.ECS_World.Components.Geometry;
using Scenes.SessionRework.Scripts.ECS_World.Components.Players;
using UnityEngine;

namespace Scenes.SessionRework.Scripts.ECS_World.Features.PlayersCinematic.Systems
{
    public class RotateWhenMovingSystem: ISystem
    {
        public World World { get; set; }

        private Filter _filter;

        private Stash<VelocityComponent> _velocityStash;
        private Stash<PositionComponent> _positionStash;
        private Stash<LookAtComponent> _lookAtStash;
        private Stash<RotationComponent> _rotationStash;
        
        public RotateWhenMovingSystem(World world)
        {
            World = world;
        }

        public void OnAwake()
        {
            _filter = World.Filter
                .With<PositionComponent>()
                .With<VelocityComponent>()
                .With<LookAtComponent>()
                .With<RotationComponent>()
                .Build();
            
            _velocityStash = World.GetStash<VelocityComponent>();
            _positionStash = World.GetStash<PositionComponent>();
            _lookAtStash =  World.GetStash<LookAtComponent>();
            _rotationStash = World.GetStash<RotationComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var velocityComponent = ref _velocityStash.Get(entity);
                ref var positionComponent = ref _positionStash.Get(entity);
                ref var lookAtComponent = ref _lookAtStash.Get(entity);
                ref var rotationComponent = ref _rotationStash.Get(entity);

                var vectorSize = velocityComponent.WorldVelocity.magnitude;

                if (vectorSize <= 0.0001f)
                    continue;

                Vector3 forwardLookDirection = lookAtComponent.Position - positionComponent.Position;

                forwardLookDirection.y = 0f;
                if (forwardLookDirection != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(forwardLookDirection);
                    rotationComponent.Rotation = targetRotation;
                }
            }
        }
        
        public void Dispose()
        {
            
        }
    }
}