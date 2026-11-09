using UnityEngine;

public class StopAiming : StateMachineBehaviour
{
    private ArcherReferences references;
    private BowIKController bowIKController;
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (references == null)
        {
            references = animator.GetComponent<ArcherReferences>();
        }
        
        if (bowIKController == null)
        {
            bowIKController = animator.GetComponent<BowIKController>();
        }

        if (references == null)
            return;

        references.Arrow.SetActive(false);

    }

     public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (references != null)
        {
            references.Arrow.SetActive(false);
        }

        if (bowIKController != null)
        {
            bowIKController.DisableBowIK();
        }
    }
}
