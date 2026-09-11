using UnityEngine;

namespace Scenes.SessionRework.Scripts.Cameras.SettingsDef.interfaces
{
    public interface ICameraLookable
    {
        public Transform CameraStartPosition { get; }
        public Transform CameraTargetLook { get; }
    }
}