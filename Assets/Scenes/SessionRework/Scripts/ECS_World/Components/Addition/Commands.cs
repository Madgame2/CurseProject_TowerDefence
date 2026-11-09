using Scenes.SessionRework.Scripts.ECS_World.Features.DTO.Enums;
using UnityEngine;

namespace Scenes.SessionRework.Scripts.ECS_World.Components.Addition
{
    public struct PlayerStateSnapShoot
    {
        public uint Tick;
        public Vector3 MoveDirection;
        public Quaternion Rotation;
        public Vector3 LookAtPoint;
        public ActionFlags Actions;
    }
}