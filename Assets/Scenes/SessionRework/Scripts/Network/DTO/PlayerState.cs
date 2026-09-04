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
        
        public PlayerState(uint objectId, Vector3 position)
        {
            ObjectId = objectId;
            Position = position;
        }
        
        public int Serialize(Span<byte> buffer)
        {
            MemoryMarshal.Write(buffer.Slice(0), ref Unsafe.AsRef(in ObjectId));

            ref byte posBuffer = ref buffer[4];
            MemoryMarshal.Write(MemoryMarshal.CreateSpan(ref posBuffer, 12), ref Unsafe.AsRef(in Position));

            return 16;
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

            return offset;
        }
    }
}