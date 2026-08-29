using System;
using System.IO;

namespace Common.Services.Net.Contracts
{
    public interface IUdpPacket
    {
        uint UdpToken { get; }
        PacketType Type { get; }
        int Serialize(Span<byte> buffer);
    }
}