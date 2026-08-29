using System;
using System.Runtime.InteropServices;
using System.Text;
using Common.Services.Net.Contracts;
using UnityEngine;

namespace Scenes.SessionRework.Scripts.Network.DTO
{
    public struct PlayerStateSnapshot : IServerUdpPaket
    {
        public PacketType Type => PacketType.PlayerWorldState;
        public string UserId;
        public uint ServerTick;
        public Vector3 Position;

        public PlayerStateSnapshot(string userId, uint serverTick, Vector3 position)
        {
            UserId = userId;
            Position = position;
            ServerTick = serverTick;
        }

        public int Serialize(Span<byte> buffer)
        {
            int offset = 0;
            
            buffer[offset] = (byte)Type;
            offset += 1;
            
            int stringByteLength = Encoding.UTF8.GetByteCount(UserId);
            ushort length = (ushort)stringByteLength;
            MemoryMarshal.Write(buffer.Slice(offset), ref length);
            offset += 2;
            
            Encoding.UTF8.GetBytes(UserId, buffer.Slice(offset));
            offset += stringByteLength;
            
            MemoryMarshal.Write(buffer.Slice(offset), ref ServerTick);
            offset += 4;
            
            float x = Position.x;
            float y = Position.y;
            float z = Position.z;

            MemoryMarshal.Write(buffer.Slice(offset), ref x);
            offset += 4;

            MemoryMarshal.Write(buffer.Slice(offset), ref y);
            offset += 4;

            MemoryMarshal.Write(buffer.Slice(offset), ref z);
            offset += 4;

            return offset;
        }

        public void Deserialize(ReadOnlySpan<byte> data)
        {
            int offset = 0;
            
            ushort length = MemoryMarshal.Read<ushort>(data.Slice(offset));
            offset += 2;
            
            UserId = Encoding.UTF8.GetString(data.Slice(offset, length));
            offset += length;
            
            ServerTick = MemoryMarshal.Read<uint>(data.Slice(offset));
            offset += 4;
            
            float x = MemoryMarshal.Read<float>(data.Slice(offset));
            offset += 4;
        
            float y = MemoryMarshal.Read<float>(data.Slice(offset));
            offset += 4;
        
            float z = MemoryMarshal.Read<float>(data.Slice(offset));
            offset += 4;

            Position = new Vector3(x, y, z);
        }
    }
}