using System;
using System.Buffers.Binary;
using Common.Services.Net.Contracts;

namespace Scenes.SessionRework.Scripts.Network.DTO
{
    public struct AcknowledgePacket : IUdpPacket
    {
        public uint UdpToken { get; }
        public PacketType Type => PacketType.Acknowledge;
        
        public static int PaketSize = sizeof(uint)+sizeof(PacketType);
        
        public int Serialize(Span<byte> buffer)
        {
            int offset = 0;
            
            BinaryPrimitives.WriteUInt32LittleEndian(buffer.Slice(offset), UdpToken);
            offset += 4;

            buffer[offset] = (byte)Type;
            offset += 1;
            
            return offset;
        }
    }
}