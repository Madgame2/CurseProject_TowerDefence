using UnityEngine;

namespace Scenes.SessionRework.Scripts.Player.View.Animations
{
    public class CharacterAnimatorIKView: MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private PlayerView viewObject;

        private const float _lookTargetWeight = 0.5f;
        
        
        private void OnAnimatorIK(int layerIndex)
        {
            if (_lookTargetWeight <= 0.001f)
            {
                animator.SetLookAtWeight(0f);
                return;
            }

            animator.SetLookAtPosition(viewObject.PlayerLooTarget.position);
            animator.SetLookAtWeight(_lookTargetWeight, 0.45f, 1f, 0f, 0.45f); 
        }
    }
}