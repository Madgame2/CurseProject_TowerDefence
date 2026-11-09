using Scenes.SessionRework.Scripts.Cameras.Components;
using Scenes.SessionRework.Scripts.Cameras.Interfaces;
using Scenes.SessionRework.Scripts.Cameras.StateMachine.States.Base;
using Scenes.SessionRework.Scripts.Cameras.StateMachine.States.Meta;
using Scenes.SessionRework.Scripts.GameWorld.View;
using UnityEngine;

namespace Scenes.SessionRework.Scripts.Cameras.StateMachine.States.AimingState
{
    public class AimingCameraState : IState
{
    public static CameraStateType CameraState => CameraStateType.Aiming;

    private readonly CameraView _camera;
    private readonly ICameraInput _input;
    private readonly CameraAnchors _cameraAnchors;

    private readonly float _sensitivity;
    private readonly float _minPitch;
    private readonly float _maxPitch;
    private readonly float _distanceMultiplier;

    private float _yaw;
    private float _pitch;

    public AimingCameraState(
        CameraView camera,
        CameraAnchors cameraAnchors,
        ICameraInput input,
        float distanceMultiplier = 0.5f,
        float sensitivity = 2f,
        float minPitch = -30f,
        float maxPitch = 70f)
    {
        _camera = camera;
        _cameraAnchors = cameraAnchors;
        _input = input;

        _distanceMultiplier = distanceMultiplier;
        _sensitivity = sensitivity;
        _minPitch = minPitch;
        _maxPitch = maxPitch;
    }

    public void Enter()
    {
        InitializeRotation();
        UpdatePosition();
    }

    public void Update(float deltaTime)
    {
        if (_cameraAnchors == null)
            return;

        UpdateRotation();
        UpdatePosition();
    }

    public void Exit()
    {
    }

    private void InitializeRotation()
    {
        Vector3 eulerAngles = _cameraAnchors.TransfromAnchor.localEulerAngles;

        _yaw = eulerAngles.y;

        // Unity возвращает углы в диапазоне 0..360.
        // Преобразуем их в диапазон -180..180.
        _pitch = eulerAngles.x > 180f
            ? eulerAngles.x - 360f
            : eulerAngles.x;

        _pitch = Mathf.Clamp(_pitch, _minPitch, _maxPitch);
    }

    private void UpdateRotation()
    {
        Vector2 mouseDelta = _input.MouseDelta;

        _yaw += mouseDelta.x * _sensitivity;
        _pitch -= mouseDelta.y * _sensitivity;

        _yaw %= 360f;

        _pitch = Mathf.Clamp(
            _pitch,
            _minPitch,
            _maxPitch);

        _cameraAnchors.TransfromAnchor.localRotation = Quaternion.Euler(
            _pitch,
            _yaw,
            0f
        );
    }

    private void UpdatePosition()
    {
        Vector3 lookPosition = _cameraAnchors.LookAnchor.position;
        Vector3 followPosition = _cameraAnchors.FollowAnchor.position;

        Vector3 direction = followPosition - lookPosition;

        Vector3 offset = direction * _distanceMultiplier;

        _camera.transform.position = lookPosition + offset;
        _camera.transform.LookAt(lookPosition);
    }
}
}