using UnityEngine;
using UnityEngine.Animations.Rigging;

public class BowIKController : MonoBehaviour
{
    [SerializeField] private Rig bowRig;
    [SerializeField] private float blendSpeed = 5f;

    private float _targetWeight;

    public void EnableBowIK()
    {
        _targetWeight = 1f;
    }

    public void DisableBowIK()
    {
        _targetWeight = 0f;
    }

    private void Update()
    {
        bowRig.weight = Mathf.MoveTowards(
            bowRig.weight,
            _targetWeight,
            blendSpeed * Time.deltaTime
        );
    }
}
