using System;
using Scenes.SessionRework.Scripts.Cameras.StateMachine.States.AimingState;
using Scenes.SessionRework.Scripts.Cameras.StateMachine.States.Meta;
using UnityEngine;

namespace Scenes.SessionRework.Scripts.Cameras.StateMachine.States.FollowCharacterDeffoultState
{
    [CreateAssetMenu(
        fileName = "AimingCameraConfig",
        menuName = "Camera/State Machine/AimingCameraConfig")]
    public class AimingCameraConfig: CameraStateConfig<AimingCameraState>
    {

        [SerializeField, Range(0.1f, 1f)]
        private float _distanceMultiplier = 0.7f;
        
        [SerializeField, Min(0f)]
        private float _sensitivity = 2f;

        [Header("Vertical Rotation")]
        [SerializeField]
        private float _minPitch = -30f;

        [SerializeField]
        private float _maxPitch = 70f;
        
        public float Sensitivity => _sensitivity;
        public float MinPitch => _minPitch;
        public float MaxPitch => _maxPitch;
        public float DistanceMultiplier => _distanceMultiplier;

    }
}