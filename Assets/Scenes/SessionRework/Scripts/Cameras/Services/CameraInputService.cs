using Scenes.SessionRework.Scripts.Cameras.Interfaces;
using UnityEngine;

namespace Scenes.SessionRework.Scripts.Cameras.Services
{
    public class CameraInputService : ICameraInput
    {
        public Vector2 MouseDelta { get; private set; }

        public void SetLookDelta(Vector2 delta)
        {
            MouseDelta = delta;
        }
    }
}