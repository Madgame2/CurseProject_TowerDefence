using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class DebugPlayerMovement : MonoBehaviour
{
    [Inject] private BaseInputActions inputActions;

    [SerializeField] private GameObject playerDebug;
    [SerializeField] private float moveSpeed = 5f;

    private Vector3? targetPoint; // текущая цель (nullable)

    private void OnEnable()
    {
        inputActions.Enable();
        inputActions.SessionPlayer.RightClick.performed += OnMove;
    }

    private void OnDisable()
    {
        inputActions.SessionPlayer.RightClick.performed -= OnMove;
        inputActions.Disable();
    }

    private void Update()
    {
        if (targetPoint.HasValue)
        {
            MovePlayer();
        }
    }

    private void OnMove(InputAction.CallbackContext ctx)
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            targetPoint = hit.point;

            // Спавним сферу в точке клика
            SpawnSphere(hit.point);
        }
    }

    private void MovePlayer()
    {
        Vector3 currentPos = playerDebug.transform.position;
        Vector3 target = targetPoint.Value;

        // Движение
        playerDebug.transform.position = Vector3.MoveTowards(
            currentPos,
            target,
            moveSpeed * Time.deltaTime
        );

        // Поворот в сторону движения (опционально, но полезно)
        Vector3 direction = (target - currentPos).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            playerDebug.transform.rotation = Quaternion.Slerp(
                playerDebug.transform.rotation,
                targetRotation,
                10f * Time.deltaTime
            );
        }

        // Если дошли — убираем цель
        if (Vector3.Distance(currentPos, target) < 0.1f)
        {
            targetPoint = null;
        }
    }

    private void SpawnSphere(Vector3 position)
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = position;

        // чуть уменьшим, чтобы выглядело аккуратнее
        sphere.transform.localScale = Vector3.one * 0.3f;
    }
}

