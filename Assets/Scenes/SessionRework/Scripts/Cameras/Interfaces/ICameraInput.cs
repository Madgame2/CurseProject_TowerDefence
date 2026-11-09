using UnityEngine;

namespace Scenes.SessionRework.Scripts.Cameras.Interfaces
{
    public interface ICameraInput
    {
        Vector2 MouseDelta { get; }
    }
}