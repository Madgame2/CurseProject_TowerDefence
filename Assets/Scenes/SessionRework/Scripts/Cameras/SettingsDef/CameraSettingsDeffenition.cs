using Scenes.SessionRework.Scripts.Cameras.SettingsDef.interfaces;
using UnityEngine;

[CreateAssetMenu(fileName = "CameraSettingsDefinition", menuName = "Camera/CameraSettingsDeffenition")]
public class CameraSettingsDefinition : ScriptableObject
{
    [SerializeField] private float _maxPitch = 60;
    [SerializeField] private float _minPitch = -10;
    
    public float MaxPitch => _maxPitch;
    public float MinPitch => _minPitch;
}
