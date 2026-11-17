using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class PlayAnimationTest : MonoBehaviour
{
    [SerializeField] private Animator animator;

    [SerializeField] private Transform anchor;
    // Update is called once per frame
    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            animator.SetTrigger("Shoot");
            Debug.Log(anchor.position);
        }
        else
        {
            animator.ResetTrigger("Shoot");
        }
    }
}
