using System;
using UnityEngine;

namespace Scenes.SessionRework.Scripts.ECS_World.Factories.Player.Model
{
    public struct PlayerData
    {
        public Guid Id;
        public Vector3 Position;
        public bool IsPlaying;
    }
}