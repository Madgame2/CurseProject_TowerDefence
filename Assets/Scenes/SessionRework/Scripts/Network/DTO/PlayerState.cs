using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Scenes.SessionRework.Scripts.Player.Enums;
using UnityEngine;

namespace Scenes.SessionRework.Scripts.Network.DTO
{
    public struct PlayerState
    {
        public uint ObjectId;
        public Vector3 Position;
        public Quaternion Rotation;
        public Vector3 LookAt;
        public Vector3 Velocity;
        public MovementState MovementState;

        public const int SerializedSize = 57;

        public PlayerState(
            uint objectId,
            Vector3 position,
            Quaternion rotation,
            Vector3 lookAt,
            Vector3 velocity,
            MovementState movementState)
        {
            ObjectId = objectId;
            Position = position;
            Rotation = rotation;
            LookAt = lookAt;
            Velocity = velocity;
            MovementState = movementState;
        }

        public int Serialize(Span<byte> buffer)
        {
            if (buffer.Length < SerializedSize)
                return 0;

            MemoryMarshal.Write(buffer.Slice(0), ref ObjectId);
            MemoryMarshal.Write(buffer.Slice(4), ref Position);
            MemoryMarshal.Write(buffer.Slice(16), ref Rotation);
            MemoryMarshal.Write(buffer.Slice(32), ref LookAt);
            MemoryMarshal.Write(buffer.Slice(44), ref Velocity);

            buffer[56] = (byte)MovementState;

            return SerializedSize;
        }

        public int Deserialize(ReadOnlySpan<byte> data)
        {
            if (data.Length < SerializedSize)
                return 0;

            ObjectId = MemoryMarshal.Read<uint>(data.Slice(0));
            Position = MemoryMarshal.Read<Vector3>(data.Slice(4));
            Rotation = MemoryMarshal.Read<Quaternion>(data.Slice(16));
            LookAt = MemoryMarshal.Read<Vector3>(data.Slice(32));
            Velocity = MemoryMarshal.Read<Vector3>(data.Slice(44));
            MovementState = (MovementState)data[56];

            return SerializedSize;
        }
    }
}