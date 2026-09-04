using System;

namespace Common.Services.Net.Services.PacketProcessor
{
    public interface IPacketProcessor
    {
        void Process(ReadOnlySpan<byte> payload);
        void AddHandler(Delegate handler);
        void RemoveHandler(Delegate handler);
    }
}