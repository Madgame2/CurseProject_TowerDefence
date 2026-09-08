using System;
using Scellecs.Morpeh;
using Scenes.SessionRework.Scripts.ECS_World.Components.Animations;
using Scenes.SessionRework.Scripts.ECS_World.Components.Geometry;
using Scenes.SessionRework.Scripts.ECS_World.Components.Players;
using Unity.Mathematics;
using UnityEngine;

namespace Scenes.SessionRework.Scripts.ECS_World.Systems.Animation
{
    public class PlayerAnimationSystem: ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        
        private Stash<AnimatorReferenceComponent> _animatorReferenceStash;
        private Stash<RotationComponent> _rotationStash;
        private Stash<VelocityComponent> _velocityStash;


        public PlayerAnimationSystem(World world)
        {
            World = world;
        }

        public void OnAwake()
        {
            _filter = World.Filter
                .With<PlayerComponent>()
                .With<RotationComponent>()
                .With<VelocityComponent>()
                .With<AnimatorReferenceComponent>()
                .Build();
            
            _animatorReferenceStash = World.GetStash<AnimatorReferenceComponent>();
            _rotationStash =  World.GetStash<RotationComponent>();
            _velocityStash = World.GetStash<VelocityComponent>();
        }
        
        public void OnUpdate(float deltaTime)
        {
            foreach (var playerEntity in _filter)
            {
                ref var animatorRefreshComponent = ref _animatorReferenceStash.Get(playerEntity);
                ref var rotationComponent = ref _rotationStash.Get(playerEntity);
                ref var velocityComponent = ref _velocityStash.Get(playerEntity);


                var animRef = animatorRefreshComponent.Animator;
                if (animRef == null)
                {
                    continue;
                }
                
                Vector3 localVelocity = math.rotate(math.inverse(rotationComponent.Rotation), velocityComponent.WorldVelocity);
                
                float moveX = localVelocity.x;
                float moveY = localVelocity.z;
                
                animRef.SetFloat(AnimatorReferenceComponent.MoveXHash, moveX, 0.1f, deltaTime);
                animRef.SetFloat(AnimatorReferenceComponent.MoveYHash, moveY, 0.1f, deltaTime);
            }
        }
        
        public void Dispose()
        {
            
        }
    }
}