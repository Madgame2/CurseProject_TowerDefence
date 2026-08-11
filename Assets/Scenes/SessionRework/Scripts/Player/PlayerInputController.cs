using Scenes.SessionRework.Scripts.Player;
using UnityEngine;
using Zenject;

public class PlayerInputController : MonoBehaviour
{
    [Inject] private readonly BaseInputActions _inputs;
    [Inject] private readonly MouseLookSettings _lookSettings;
    [Inject] private readonly IPlayerInputHandler _playerInputHandler;

    private Vector3 _lastDirection = Vector3.zero;
    private float _xRotation = 0f;

    private void Awake()
    {
        _inputs.SessionPlayer.Enable();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Update()
    {
        HandleMovement();
        HandleLook();
    }

    private void HandleLook()
    {
        Vector2 lookInput = _inputs.SessionPlayer.Look.ReadValue<Vector2>();

        if (lookInput != Vector2.zero)
        {
            _playerInputHandler.ProcessPlayerLook(lookInput);
        }
    }

    private void HandleMovement()
    {
        Vector2 moveInput = _inputs.SessionPlayer.Move.ReadValue<Vector2>();
        Vector3 direction = new Vector3(moveInput.x, 0, moveInput.y).normalized;

        if (_lastDirection != direction)
        {
            _lastDirection = direction;
            _playerInputHandler.ProcessPlayerMovemnet(direction);
        }
    }
}
