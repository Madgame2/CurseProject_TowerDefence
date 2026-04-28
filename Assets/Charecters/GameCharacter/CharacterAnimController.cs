using Scenes.Lobby.Entities;
using UnityEngine;

using UnityEngine;

public class CharacterAnimController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform player;

    public void SetVelocity(Vector3 velocity)
    {
        Vector3 horizontal = velocity;
        horizontal.y = 0;

        float speed = horizontal.magnitude;

        animator.SetFloat("Speed", speed);
    }
}
