using Scellecs.Morpeh;
using Scenes.SessionRework.Scripts.Cameras.Components;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Scenes.SessionRework.Scripts.Player.View
{
    public class PlayerView: MonoBehaviour
    {
        [SerializeField] private CameraSettingsDefinition _cameraSettingsDefinition;
        [SerializeField] private Animator _animator;
        [SerializeField] private Transform _spineBone;
        [SerializeField] private Transform _characterFacing;
        [SerializeField] private  Transform _lookTarget;
        [SerializeField]private CameraAnchors _defoultStateCameraAnchors;
        [SerializeField] private CameraAnchors _aimingStateCameraAnchors;

        public bool IsAiming;
        
        public Transform SpineBone => _spineBone;
        public CameraSettingsDefinition  CameraSettingsDefinition { get => _cameraSettingsDefinition; }
        public Animator Animator => _animator;
        public CameraAnchors DeffoultStateCameraAnchors =>_defoultStateCameraAnchors;
        public CameraAnchors AimingStateCameraAnchors =>_aimingStateCameraAnchors;
        public Transform PlayerLookTarget => _lookTarget;
        public Transform CharacterFacing => _characterFacing;
    }
}