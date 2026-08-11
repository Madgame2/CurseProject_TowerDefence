using UnityEngine;

public class TheardPersoneCameraController : MonoBehaviour
{
    private Transform _camera;
    [SerializeField] private Transform _target;

    [SerializeField] private float _distance = 2.0f;

    public void Awake()
    {
        _camera = Camera.main.transform;
    }

    public void Update()
    {
        if (_camera == null || _target == null) return;

        Vector3 heading = _target.position - _camera.position;

        Vector3 direction = heading.normalized;

        _camera.position = _target.position - direction* _distance;


        _camera.LookAt(_target);

    }

    public void SetTaget(Transform target)
    {
        _target = target;
    }
}
