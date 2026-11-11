using Scellecs.Morpeh;
using UnityEngine;

namespace Scenes.SessionRework.Scripts.ECS_World.Features.Components
{

    public struct RotateToRequest : IComponent
    {
        public Quaternion NewRotation;
        public float Angle;
        public bool IsStarted;
    }
}