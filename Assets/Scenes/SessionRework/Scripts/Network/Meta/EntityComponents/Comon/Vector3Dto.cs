using System;
using UnityEngine;

namespace Scenes.SessionRework.Scripts.Network.Meta.EntityComponents.SpawnPoint.DTO
{
    [Serializable]
    public struct Vector3Dto
    {
        public float X;
        public float Y;
        public float Z;

        public Vector3Dto(Vector3 value)
        {
            X = value.x;
            Y = value.y;
            Z = value.z;
        }
    }
}