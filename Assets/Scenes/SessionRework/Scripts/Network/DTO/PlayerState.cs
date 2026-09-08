using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Scenes.SessionRework.Scripts.Network.DTO
{
    public struct PlayerState
    {
        public uint ObjectId;
        public Vector3 Position;
        public Vector3 Velocity;
        
        public PlayerState(uint objectId, Vector3 position, Vector3 velocity)
        {
            ObjectId = objectId;
            Position = position;
            Velocity = velocity;
        }
        
        public int Serialize(Span<byte> buffer)
        {
            MemoryMarshal.Write(buffer.Slice(0), ref Unsafe.AsRef(in ObjectId));

            ref byte posBuffer = ref buffer[4];
            MemoryMarshal.Write(MemoryMarshal.CreateSpan(ref posBuffer, 12), ref Unsafe.AsRef(in Position));


            posBuffer = ref buffer[16];
            MemoryMarshal.Write(MemoryMarshal.CreateSpan(ref posBuffer, 12), ref Unsafe.AsRef(in Velocity));

            return 28;
        }
        
        public int Deserialize(ReadOnlySpan<byte> data)
        {
            int offset = 0;
            
            ObjectId = MemoryMarshal.Read<uint>(data.Slice(offset));
            offset += 4;
            
            float x = MemoryMarshal.Read<float>(data.Slice(offset));
            offset += 4;

            float y = MemoryMarshal.Read<float>(data.Slice(offset));
            offset += 4;

            float z = MemoryMarshal.Read<float>(data.Slice(offset));
            offset += 4;

            Position = new Vector3(x, y, z); 
            
            x = MemoryMarshal.Read<float>(data.Slice(offset));
            offset += 4;

            y = MemoryMarshal.Read<float>(data.Slice(offset));
            offset += 4;

            z = MemoryMarshal.Read<float>(data.Slice(offset));
            offset += 4;

            Velocity = new Vector3(x, y, z); 

            return offset;
        }
    }
}