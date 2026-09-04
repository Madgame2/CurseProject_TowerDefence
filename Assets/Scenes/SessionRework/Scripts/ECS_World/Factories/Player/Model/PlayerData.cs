using System;
using UnityEngine;

namespace Scenes.SessionRework.Scripts.ECS_World.Factories.Player.Model
{
    public struct PlayerData
    {
        public string PlayerId;
        public uint ObjectId;
        public Vector3 Position;
        public bool IsPlaying;
    }
}