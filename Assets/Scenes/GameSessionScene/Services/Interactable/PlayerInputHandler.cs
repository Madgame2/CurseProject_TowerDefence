using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class PlayerInputHandler : MonoBehaviour
{
    [Inject] private BaseInputActions inputActions;
    [Inject] private PlayerMovementController _MovementController;

    [SerializeField] private GameObject playerDebug;

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

    private void OnMove(InputAction.CallbackContext ctx)
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();

        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 worldPoint = hit.point;

            _MovementController.MoveTo(playerDebug.transform, worldPoint);
        }
    }
}
