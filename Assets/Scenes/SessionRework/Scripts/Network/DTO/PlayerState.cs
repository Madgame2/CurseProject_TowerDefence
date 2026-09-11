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
        public Vector3 Velocity;
        public MovementState MovementState;
        
        public const int SerializedSize = 29;

        
        public PlayerState(uint objectId, Vector3 position, Vector3 velocity,  MovementState movementState)
        {
            ObjectId = objectId;
            Position = position;
            Velocity = velocity;
            MovementState = movementState;
        }
        
        public int Serialize(Span<byte> buffer)
        {
            if (buffer.Length < SerializedSize)
                return 0;

            MemoryMarshal.Write(buffer.Slice(0), ref ObjectId);
            MemoryMarshal.Write(buffer.Slice(4), ref Position);
            MemoryMarshal.Write(buffer.Slice(16), ref Velocity);
            buffer[28] = (byte)MovementState;

            return SerializedSize;
        }
        
        public int Deserialize(ReadOnlySpan<byte> data)
        {
            if (data.Length < SerializedSize) 
                return 0;

            ObjectId = MemoryMarshal.Read<uint>(data.Slice(0));
            Position = MemoryMarshal.Read<Vector3>(data.Slice(4));
            Velocity = MemoryMarshal.Read<Vector3>(data.Slice(16));
            MovementState = (MovementState)data[28];

            return SerializedSize;
        }
    }
}