using UnityEngine;

namespace Scenes.SessionRework.Scripts.ECS_World.Components.Addition
{
    public struct MoveInputCommand
    {
        public uint Tick;
        public Vector3 MoveDirection;
    }
}