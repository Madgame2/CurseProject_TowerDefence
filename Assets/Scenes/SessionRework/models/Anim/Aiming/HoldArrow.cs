using UnityEngine;

public class HoldArrow : StateMachineBehaviour
{
    private ArcherReferences references;
    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (references == null)
        {
            references = animator.GetComponent<ArcherReferences>();
        }

        
        if(references!=null)
        {
            references.Arrow.SetActive(true);
        }
    }
}
