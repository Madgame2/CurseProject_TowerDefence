using UnityEngine;

public class TakeArrow : StateMachineBehaviour
{
    private ArcherReferences references;
    

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (references == null)
        {
            references = animator.GetComponent<ArcherReferences>();
        }
       

        if (references == null)
            return;

        references.Arrow.SetActive(true);
    }


}
