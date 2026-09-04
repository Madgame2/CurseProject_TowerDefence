using System;

namespace Common.Services.Net.Contracts
{
    public interface IServerUdpPaket
    {
        PacketType Type { get; }

        int Serialize(Span<byte> buffer);
        
        void Deserialize(ReadOnlySpan<byte> data);
        
        void Release();
    }
}