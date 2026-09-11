using Scellecs.Morpeh;
using Scenes.SessionRework.Scripts.ECS_World.Components.Animations;
using Scenes.SessionRework.Scripts.ECS_World.Components.Geometry;
using Scenes.SessionRework.Scripts.ECS_World.Components.Movement;
using Scenes.SessionRework.Scripts.ECS_World.Components.Players;
using Scenes.SessionRework.Scripts.Player.Enums;

namespace Scenes.SessionRework.Scripts.ECS_World.Features.PlayersCinematic.Systems
{
    public class PlayerJumpAnimationSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;

        private Stash<AnimatorReferenceComponent> _animatorReference;
        private Stash<VelocityComponent> _velocityStash;
        private Stash<MovementStateComponent> _movementState;

        public PlayerJumpAnimationSystem(World world)
        {
            World = world;
        }

        public void OnAwake()
        {
            _animatorReference = World.GetStash<AnimatorReferenceComponent>();
            _velocityStash = World.GetStash<VelocityComponent>();
            _movementState = World.GetStash<MovementStateComponent>();

            _filter = World.Filter
                .With<PlayerComponent>()
                .With<AnimatorReferenceComponent>()
                .With<VelocityComponent>()
                .With<MovementStateComponent>()
                .Build();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var animatorComponent = ref _animatorReference.Get(entity);
                ref var velocityComponent = ref _velocityStash.Get(entity);
                ref var movementStateComponent = ref _movementState.Get(entity);

                var animator = animatorComponent.Animator;
                if (animator == null)
                    continue;

                var verticalVelocity = velocityComponent.WorldVelocity.y;
                var movementState = movementStateComponent.MovementState;
            
                var isGrounded = movementState == MovementState.Grounded;

                animator.SetFloat(AnimatorReferenceComponent.VerticalVelocity, verticalVelocity);
                animator.SetBool(AnimatorReferenceComponent.IsGrounded, isGrounded);
                
                if (movementState == MovementState.Jumping)
                {
                    animator.SetTrigger(AnimatorReferenceComponent.JumpTrigger);
                }
                else if (isGrounded)
                {
                    animator.ResetTrigger(AnimatorReferenceComponent.JumpTrigger);
                }
            }
        }

        public void Dispose()
        {
        }
    }
}