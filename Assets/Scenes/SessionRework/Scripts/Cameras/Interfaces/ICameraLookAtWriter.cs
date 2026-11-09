using UnityEngine;

namespace Scenes.SessionRework.Scripts.Cameras.Interfaces
{
    public interface ICameraLookAtWriter
    {
        void SetLookAt(Vector3 cameraLookAtPoint);
    }
}