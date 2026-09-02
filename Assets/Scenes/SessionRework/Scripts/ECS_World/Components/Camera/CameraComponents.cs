using Scellecs.Morpeh;
using UnityEngine;

namespace Scenes.SessionRework.Scripts.ECS_World.Components.Camera
{
    public struct LookInputComponent : IComponent
    {
        public Vector2 Delta;
    }

    public struct RotationStateComponent : IComponent
    {
        public float Yaw;         // Горизонтальный угол (вокруг оси Y)
        public float Pitch;       // Вертикальный угол (вокруг оси X)
        
        public float MinPitch;    
        public float MaxPitch;
        
        public float Sensitivity;
    }

    public struct CameraTargetComponent : IComponent
    {
        public Vector3 PivotOffset;
        public float TargetDistance;
        public float SmoothTime;
    }

    public struct CameraViewComponent : IComponent
    {
        public Transform CameraTransform;
    }
}