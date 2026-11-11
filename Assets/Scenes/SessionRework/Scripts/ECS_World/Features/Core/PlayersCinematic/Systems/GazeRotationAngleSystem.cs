using Scellecs.Morpeh;
using Scenes.SessionRework.Scripts.ECS_World.Components.Common;
using Scenes.SessionRework.Scripts.ECS_World.Components.Geometry;
using Scenes.SessionRework.Scripts.ECS_World.Components.Players;
using Scenes.SessionRework.Scripts.ECS_World.Features.Components;
using UnityEngine;

namespace Scenes.SessionRework.Scripts.ECS_World.Features.PlayersCinematic.Systems
{
    public class GazeRotationAngleSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        
        private Stash<RotationComponent> _rotationStash;
        private Stash<LookAtComponent> _lookAtStash;
        private Stash<PositionComponent> _positionStash;
        private Stash<RotateToRequest> _rotateToRequestStash;
        
        private const float TURN_THRESHOLD = 80f;
        
        public GazeRotationAngleSystem(World world)
        {
            World = world;
        }

        public void OnAwake()
        {
            _filter = World.Filter
                .With<PlayerComponent>()
                .With<PositionComponent>()
                .With<RotationComponent>()
                .With<LookAtComponent>()
                .Without<RotateToRequest>()
                .Build();
            
            _rotationStash = World.GetStash<RotationComponent>();
            _positionStash = World.GetStash<PositionComponent>();
            _lookAtStash = World.GetStash<LookAtComponent>();
            _rotateToRequestStash = World.GetStash<RotateToRequest>();
        }
        
        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var rotationComponent = ref _rotationStash.Get(entity);
                ref var lookAtComponent = ref _lookAtStash.Get(entity);
                ref var positionComponent = ref _positionStash.Get(entity);
                
                
                var lookDirection = lookAtComponent.Position -  positionComponent.Position;
                lookDirection.y = 0;
                
                if (lookDirection.sqrMagnitude < 0.0001f)
                {
                    continue;
                }
                
                var rotationDirection = rotationComponent.Rotation * Vector3.forward;
                var angle = Vector3.SignedAngle(lookDirection, rotationDirection, Vector3.up);
                
                if (Mathf.Abs(angle) > TURN_THRESHOLD)
                {
                    var targetRotation = Quaternion.LookRotation(lookDirection, Vector3.up);
                    
                    _rotateToRequestStash.Set(entity, new RotateToRequest
                    {
                        NewRotation = targetRotation,
                        Angle = angle
                    });
                }
            }
        }
        
        public void Dispose()
        {
            
        }
    }
}