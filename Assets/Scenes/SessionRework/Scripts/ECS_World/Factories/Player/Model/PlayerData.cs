using System;
using UnityEngine;

namespace Scenes.SessionRework.Scripts.ECS_World.Factories.Player.Model
{
    public struct PlayerData
    {
        public uint PlayerId;
        public uint ObjectId;
        public Vector3 Position;
        public bool IsPlaying;
    }
}