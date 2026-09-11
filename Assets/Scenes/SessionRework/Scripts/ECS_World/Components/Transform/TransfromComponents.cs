using Scellecs.Morpeh;
using UnityEngine;

namespace Scenes.SessionRework.Scripts.ECS_World.Components.Geometry
{
    public struct PositionComponent : IComponent { public Vector3 Position; }
    public struct RotationComponent : IComponent { public Quaternion Rotation; }
    public struct VelocityComponent : IComponent { public Vector3 WorldVelocity; }
}