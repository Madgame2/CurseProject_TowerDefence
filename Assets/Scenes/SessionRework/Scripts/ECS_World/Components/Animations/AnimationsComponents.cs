using Scellecs.Morpeh;
using UnityEngine;

namespace Scenes.SessionRework.Scripts.ECS_World.Components.Animations
{
    public struct HumanoidBonesComponent : IComponent
    {
        public Transform SpineBone;
    }

    public struct TurnInPlaceComponent : IComponent
    {
        public float TurnThreshold;
        public float WeightBlendSpeed; 
        
        public float TotalIKWeight;    
        public float TargetIKWeight;   
        
        public bool IsTurning;
    }
    
    public struct AnimatorReferenceComponent : IComponent
    {
        public Animator Animator;
        public static readonly int MoveXHash = Animator.StringToHash("MoveX");
        public static readonly int MoveYHash = Animator.StringToHash("MoveY");
        
        public static readonly int VerticalVelocity = Animator.StringToHash("VerticalVelocity");
        public static readonly int JumpTrigger = Animator.StringToHash("Jump");
        public static readonly int IsGrounded = Animator.StringToHash("IsGrounded");
    }
}