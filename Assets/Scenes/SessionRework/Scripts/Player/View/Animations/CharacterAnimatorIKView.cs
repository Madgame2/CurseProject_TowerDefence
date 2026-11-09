using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Scenes.SessionRework.Scripts.Player.View.Animations
{
    public class CharacterAnimatorIKView: MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private PlayerView viewObject;
        
        [Header("Idle Look")]
        [SerializeField] private float lookTargetWeight = 0.5f;

        [Header("Aiming")]
        [SerializeField] private Rig aimRig;
        [SerializeField] private float aimTransitionSpeed = 5f;

        private bool _isAiming => viewObject.IsAiming;
        private float _currentAimWeight;
        
        private void Update()
        {
            float targetWeight = _isAiming ? 1f : 0f;

            _currentAimWeight = Mathf.MoveTowards(
                _currentAimWeight,
                targetWeight,
                aimTransitionSpeed * Time.deltaTime
            );

            aimRig.weight = _currentAimWeight;
        }
        
        private void OnAnimatorIK(int layerIndex)
        {
            if (_isAiming)
            {
                animator.SetLookAtWeight(0f);
                return;
            }
            
            if (lookTargetWeight <= 0.001f)
            {
                animator.SetLookAtWeight(0f);
                return;
            }

            animator.SetLookAtPosition(viewObject.PlayerLookTarget.position);
            animator.SetLookAtWeight(
                lookTargetWeight,
                0.45f,
                1f,
                0f,
                0.45f
            );
        }
    }
}