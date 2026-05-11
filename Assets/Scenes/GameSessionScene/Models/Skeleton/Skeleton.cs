using UnityEngine;

public class Skeleton : MonoBehaviour
{
    [SerializeField] private Transform lightningTarget;
    public Transform LightningTarget => lightningTarget;
}
