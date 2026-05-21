using DG.Tweening;
using UnityEngine;
using Zenject;

public class ThirdPersonCamera : MonoBehaviour
{
    [Inject] private BaseInputActions inputActions;

    [Header("Target")]
    [SerializeField] private Transform target;

    [Header("Distance")]
    [SerializeField] private float distance = 5f;
    [SerializeField] private float minDistance = 2f;
    [SerializeField] private float maxDistance = 10f;

    [Header("Rotation")]
    [SerializeField] private float sensitivity = 3f;
    [SerializeField] private float minY = -20f; // ограничение вниз
    [SerializeField] private float maxY = 60f;  // ограничение вверх

    [Header("Follow")]
    [SerializeField] private float smoothTime = 0.1f;

    [Header("Input")]
    [SerializeField] private KeyCode rotateKey = KeyCode.Mouse1; // ПКМ
    [SerializeField] private KeyCode altRotateKey = KeyCode.LeftAlt;

    [Header("Target Smoothing")]
    [SerializeField] private float targetSmoothTime = 0.15f;
    [SerializeField] private float maxTargetSpeed = 50f;

    private Vector3 smoothedTargetPosition;
    private Vector3 targetVelocity;

    private float currentX;
    private float currentY;

    private Vector3 velocity;

    private bool isTransitioning = false;

    private void Start()
    {
        if (target != null)
            smoothedTargetPosition = target.position;
    }

    void LateUpdate()
    {
        if (target == null) return;

        HandleInput();

        UpdateTargetSmoothing();

        UpdateCamera();
    }


    private void OnEnable()
    {
        inputActions?.Enable();
    }

    private void OnDisable()
    {
        inputActions?.Disable();
    }

    private void UpdateTargetSmoothing()
    {
        if (target == null) return;

        smoothedTargetPosition = Vector3.SmoothDamp(
            smoothedTargetPosition,
            target.position,
            ref targetVelocity,
            targetSmoothTime,
            maxTargetSpeed
        );
    }


    private void HandleInput()
    {
        if (inputActions == null) return;

        Vector2 look = inputActions.SessionPlayer.Look.ReadValue<Vector2>();
        float zoom = inputActions.SessionPlayer.Zoom.ReadValue<float>();

        bool isRotating =
            inputActions.SessionPlayer.Rotate.IsPressed() ||
            inputActions.SessionPlayer.AltRotate.IsPressed();

        if (isRotating)
        {
            currentX += look.x * sensitivity;
            currentY -= look.y * sensitivity;

            currentY = Mathf.Clamp(currentY, minY, maxY);
        }

        if (Mathf.Abs(zoom) > 0.01f)
        {
            distance -= zoom * 5f;
            distance = Mathf.Clamp(distance, minDistance, maxDistance);
        }
    }

    private void UpdateCamera()
    {
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        Vector3 desiredPosition = smoothedTargetPosition - rotation * Vector3.forward * distance;

        if (!isTransitioning)
        {
            transform.position = Vector3.SmoothDamp(
                transform.position,
                desiredPosition,
                ref velocity,
                smoothTime
            );
        }

        transform.LookAt(smoothedTargetPosition);
    }

    public void SetTarget(Transform newTarget)
    {
        if (newTarget == null) return;

        target = newTarget;

        // вычисляем новую позицию камеры
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        Vector3 targetPos = target.position - rotation * Vector3.forward * distance;

        isTransitioning = true;

        // плавный перелет
        transform.DOMove(targetPos, 0.5f)
            .SetEase(Ease.OutCubic)
            .OnComplete(() => isTransitioning = false);

        // плавный поворот камеры
        transform.DOLookAt(target.position, 0.5f)
            .SetEase(Ease.OutCubic);
    }
}