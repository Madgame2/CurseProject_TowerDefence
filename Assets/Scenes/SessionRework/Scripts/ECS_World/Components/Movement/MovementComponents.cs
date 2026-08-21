using Scellecs.Morpeh;
using UnityEngine;

namespace Scenes.SessionRework.Scripts.ECS_World.Components.Movement
{
    public struct InputComponent : IComponent
    {
        public uint Tick;
        public Vector3 MoveDirection;
    }
}