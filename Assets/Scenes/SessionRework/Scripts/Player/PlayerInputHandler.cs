using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Scenes.SessionRework.Scripts.Player
{
    public class PlayerInputHandler : MonoBehaviour, IPlayerInputHandler
    {
        [Inject] private readonly MouseLookSettings _lookSettings;

        [SerializeField] private float speed =1.0f;
        [SerializeField] private Transform debugTarget;
        [SerializeField] private Transform rootObject;
        [SerializeField] private Animator animator;

        [SerializeField] private Transform cameraTransform;
        [SerializeField] private Transform verticalRotationPivot;
        
        private static readonly int MoveXHash = Animator.StringToHash("MoveX");
        private static readonly int MoveYHash = Animator.StringToHash("MoveY");

        private Vector3 _localInputDirection;
        private float _xRotation;

        Task IPlayerInputHandler.ProcessPlayerMovemnet(Vector3 direction)
        {
            _localInputDirection = direction;
            return Task.CompletedTask;
        }

        public void UpdateAnimations(Vector3
            moveInput)
        {
            animator.SetFloat(MoveXHash, moveInput.x, 0.1f, Time.deltaTime);
            animator.SetFloat(MoveYHash, moveInput.z, 0.1f, Time.deltaTime);
        }


        private void Update()
        {
            Vector3 worldDirection = rootObject.rotation * _localInputDirection;

            worldDirection.y = 0f;
            if (worldDirection.sqrMagnitude > 0.001f)
            {
                worldDirection.Normalize();
            }

            debugTarget.position += worldDirection * (speed * Time.deltaTime);
            UpdateAnimations(_localInputDirection);
        }

        public Task ProcessPlayerLook(Vector2 lookDelta)
        {
            float mouseX = lookDelta.x * _lookSettings.Sensitivity * Time.deltaTime;
            float mouseY = lookDelta.y * _lookSettings.Sensitivity * Time.deltaTime;

            _xRotation -= mouseY;
            _xRotation = Mathf.Clamp(_xRotation, _lookSettings.TopClamp, _lookSettings.BottomClamp);

            float currentYRotation = verticalRotationPivot.localEulerAngles.y + mouseX;
            verticalRotationPivot.localRotation = Quaternion.Euler(_xRotation, currentYRotation, 0f);

            return Task.CompletedTask;
        }
    }
}
