using UnityEngine;

namespace Scenes.SessionRework.Scripts.Player.View
{
    public class PlayerView: MonoBehaviour
    {
        [SerializeField] private CameraSettingsDefinition _cameraSettingsDefinition;
        [SerializeField] private Transform _cameraStartPosition;
        [SerializeField] private Transform _cameraTargetLook;

    
        public Transform CameraStartPosition { get =>  _cameraStartPosition; }
        public Transform CameraTargetLook { get =>  _cameraTargetLook; }
        public CameraSettingsDefinition  CameraSettingsDefinition { get => _cameraSettingsDefinition; }
    }
}