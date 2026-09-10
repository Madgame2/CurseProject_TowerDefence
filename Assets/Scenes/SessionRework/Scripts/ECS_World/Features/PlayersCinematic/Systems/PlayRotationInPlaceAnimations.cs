using Scellecs.Morpeh;
using Scenes.SessionRework.Scripts.ECS_World.Components.Animations;
using Scenes.SessionRework.Scripts.ECS_World.Components.Geometry;
using Scenes.SessionRework.Scripts.ECS_World.Components.Players;
using Scenes.SessionRework.Scripts.ECS_World.Features.Components;
using UnityEngine;

namespace Scenes.SessionRework.Scripts.ECS_World.Features.PlayersCinematic.Systems
{
    public class PlayRotationInPlaceAnimations: ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        
        private Stash<RotationComponent> _rotationStash;
        private Stash<RotateToRequest> _rotateToRequestStash;
        private Stash<AnimatorReferenceComponent> _animatorStash;
        
        private const float ROTATION_SPEED = 180f;
        
        private static readonly int TurnSpeedHash = Animator.StringToHash("TurnSpeedMultiplier");
        private static readonly int TurnRightHash = Animator.StringToHash("TurnRight90");
        private static readonly int TurnLeftHash = Animator.StringToHash("TurnLeft90");
        
        private const float ANIM_CLIP_DURATION = 0.667f; 
        private const float ANIM_CLIP_ANGLE = 90f;
        
        public PlayRotationInPlaceAnimations(World world)
        {
            World = world;
        }

        public void OnAwake()
        {
            _filter = World.Filter
                .With<AnimatorReferenceComponent>()
                .With<PlayerComponent>()
                .With<RotationComponent>()
                .With<RotateToRequest>()
                .Build();
            
            _rotationStash = World.GetStash<RotationComponent>();
            _rotateToRequestStash = World.GetStash<RotateToRequest>();
            _animatorStash = World.GetStash<AnimatorReferenceComponent>();
        }
        
        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var rotationComponent = ref _rotationStash.Get(entity);
                ref var request = ref _rotateToRequestStash.Get(entity);
                ref var animatorRef = ref _animatorStash.Get(entity);
                
                rotationComponent.Rotation = Quaternion.RotateTowards(
                    rotationComponent.Rotation,
                    request.NewRotation,
                    ROTATION_SPEED * deltaTime
                );


                if (!request.IsStarted)
                {
                    request.IsStarted = true;
                    var animator = animatorRef.Animator;

                    if (animator != null)
                    {
                        float animDegreesPerSecond = ANIM_CLIP_ANGLE / ANIM_CLIP_DURATION;
                        float speedMultiplier = ROTATION_SPEED / animDegreesPerSecond;
                        animator.SetFloat(TurnSpeedHash, speedMultiplier);
                        
                        
                        animator.ResetTrigger(TurnRightHash);
                        animator.ResetTrigger(TurnLeftHash);
                        animator.SetTrigger(request.Angle > 0 ? TurnRightHash : TurnLeftHash);
                    }
                }

                if (Quaternion.Angle(rotationComponent.Rotation, request.NewRotation) < 0.5f)
                {
                    rotationComponent.Rotation = request.NewRotation;
                    _rotateToRequestStash.Remove(entity);
                }
            }
        }
        
        public void Dispose()
        {
            
        }
        
        private void StartTurn(Animator animator,string triggerName)
        {
            if(animator == null)
                return;
            
            animator.ResetTrigger("TurnRight90");
            animator.ResetTrigger("TurnLeft90");
            
            animator.SetTrigger(triggerName);
        }
    }
}