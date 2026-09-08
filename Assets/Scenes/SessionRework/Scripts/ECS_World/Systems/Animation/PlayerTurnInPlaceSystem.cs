using Scellecs.Morpeh;
using Scenes.SessionRework.Scripts.ECS_World.Components.Animations;
using Scenes.SessionRework.Scripts.ECS_World.Components.Camera;
using Scenes.SessionRework.Scripts.ECS_World.Components.Common;
using Scenes.SessionRework.Scripts.ECS_World.Components.Geometry;
using Scenes.SessionRework.Scripts.ECS_World.Components.Players;
using UnityEngine;

namespace Scenes.SessionRework.Scripts.ECS_World.Systems.Camera
{
    public class PlayerTurnInPlaceSystem: ISystem
    {
        public World World { get; set; }

        private Filter _cameraFilter;
        private Filter _playerFilter;
        
        private Stash<RotationStateComponent> _cameraRotationStash;
        private Stash<RotationComponent> _characterRotationStash;
        private Stash<CharacterViewComponent> _characterViewStash;
        private Stash<TurnInPlaceComponent> _turnStash;
        private Stash<AnimatorReferenceComponent> _animatorStash;
        
        public PlayerTurnInPlaceSystem(World world)
        {
            World = world;
        }
        

        public void OnAwake()
        {
            _cameraFilter = World.Filter
                .With<CameraViewComponent>()
                .With<RotationStateComponent>()
                .Build();

            _playerFilter = World.Filter
                .With<PlayerComponent>()
                .With<RotationComponent>()
                .With<CharacterViewComponent>()
                .With<UnityViewComponent>()
                .With<AnimatorReferenceComponent>()
                .With<TurnInPlaceComponent>()
                .Build();

            _cameraRotationStash = World.GetStash<RotationStateComponent>();
            _characterRotationStash = World.GetStash<RotationComponent>();
            _characterViewStash = World.GetStash<CharacterViewComponent>();
            _turnStash = World.GetStash<TurnInPlaceComponent>();
            _animatorStash = World.GetStash<AnimatorReferenceComponent>();
        }


        public void OnUpdate(float deltaTime)
        {
            if (_cameraFilter.IsEmpty()) return;
            
            var cameraEntity = _cameraFilter.GetEntity(0);
            ref var cameraRotationState = ref _cameraRotationStash.Get(cameraEntity);
            float cameraYaw = cameraRotationState.Yaw;

            foreach (var playerEntity in _playerFilter)
            {
                ref var characterView = ref _characterViewStash.Get(playerEntity);
                ref var turnComponent = ref _turnStash.Get(playerEntity);
                ref var animatorComponent = ref _animatorStash.Get(playerEntity);
                
                var animator = animatorComponent.Animator;
                if (animator == null) continue;
                
                bool isMoving = animator.GetFloat("Speed") > 0.1f;
                
                float characterYaw = characterView.CharacterRoot.eulerAngles.y;

                if (isMoving)
                {
                    characterView.CharacterRoot.eulerAngles =  new Vector3(0f, cameraYaw, 0f);
                    
                    turnComponent.IsTurning = false;
                    turnComponent.TargetIKWeight = 1.0f;
                }
                else
                {
                    float angleDelta = Mathf.DeltaAngle(characterYaw, cameraYaw);

                    if (!turnComponent.IsTurning)
                    {
                        if (angleDelta > turnComponent.TurnThreshold)
                        {
                            StartTurn(animator, ref turnComponent, "TurnRight90");
                        }
                        else if (angleDelta < -turnComponent.TurnThreshold)
                        {
                            StartTurn(animator, ref turnComponent, "TurnLeft90");
                        }
                    }
                    else
                    {
                        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
                        bool isPlayingTurn = stateInfo.IsName("TurnRight90") || stateInfo.IsName("TurnLeft90");
                        
                        if (isPlayingTurn)
                        {
                            if (animator.IsInTransition(0) || stateInfo.normalizedTime > 0.95f)
                            {
                                EndTurn(ref turnComponent, ref characterView, cameraYaw, characterView.CharacterRoot);
                            }
                        }
                    }
                }
                
                turnComponent.TotalIKWeight = Mathf.MoveTowards(
                    turnComponent.TotalIKWeight,
                    turnComponent.TargetIKWeight,
                    turnComponent.WeightBlendSpeed * deltaTime
                );
            }
        }
        
        public void Dispose()
        {
            
        }
        
        private void StartTurn(Animator animator, ref TurnInPlaceComponent turnComponent, string triggerName)
        {
            animator.ResetTrigger("TurnRight90");
            animator.ResetTrigger("TurnLeft90");

            turnComponent.IsTurning = true;
            turnComponent.TargetIKWeight = 0f; 
            animator.SetTrigger(triggerName);
        }
        
        private void EndTurn(ref TurnInPlaceComponent turnComponent, ref  CharacterViewComponent characterView, float targetYaw, Transform characterTransform)
        {
            turnComponent.IsTurning = false;
            turnComponent.TargetIKWeight = 1f;
            
            characterView.CharacterRoot.eulerAngles =  new Vector3(0f, targetYaw, 0f);
        }
    }
}