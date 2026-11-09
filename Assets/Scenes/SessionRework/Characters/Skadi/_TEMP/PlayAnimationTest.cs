using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class PlayAnimationTest : MonoBehaviour
{
    [SerializeField] private Animator animator;
    
    // Update is called once per frame
    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            animator.SetTrigger("Shoot");
        }
        else
        {
            animator.ResetTrigger("Shoot");
        }
    }
}
