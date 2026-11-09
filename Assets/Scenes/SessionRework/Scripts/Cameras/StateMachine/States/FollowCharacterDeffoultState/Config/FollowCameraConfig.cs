using System;
using Scenes.SessionRework.Scripts.Cameras.StateMachine.States.Meta;
using UnityEngine;

namespace Scenes.SessionRework.Scripts.Cameras.StateMachine.States.FollowCharacterDeffoultState
{
    [CreateAssetMenu(
        fileName = "FollowStateConfig",
        menuName = "Camera/State Machine/FollowCameraConfig")]
    public class FollowCameraConfig: CameraStateConfig<FollowCameraState>
    {

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
    }
}