using System;
using System.Buffers.Binary;
using Scenes.SessionRework.Scripts.Services.Replication.Models;

namespace Scenes.SessionRework.Scripts.Network.DTO
{
    public readonly struct ReplicationLogEntry
    {
        public const int HeaderSize = 4;

        public uint Tick { get; }
        public ServerEvent Event { get; }

        public ReplicationLogEntry(uint tick, ServerEvent evt)
        {
            Tick = tick;
            Event = evt;
        }

        public int GetSize()
        {
            return HeaderSize + Event.GetSize();
        }

        public int WriteTo(Span<byte> destination)
        {
            int size = GetSize();

            if (destination.Length < size)
                throw new ArgumentException("Destination buffer is too small.");

            BinaryPrimitives.WriteUInt32LittleEndian(
                destination.Slice(0, 4), Tick);

            return HeaderSize + Event.WriteTo(destination.Slice(HeaderSize));
        }
        
        public static ReplicationLogEntry ReadFrom(
            ReadOnlySpan<byte> source,
            out int bytesRead)
        {
            if (source.Length < HeaderSize)
                throw new ArgumentException("Source buffer is too small.");

            uint tick = BinaryPrimitives.ReadUInt32LittleEndian(
                source.Slice(0, 4));

            ServerEvent evt = new ServerEvent();

            int eventBytes = evt.ReadFrom(source.Slice(HeaderSize));

            bytesRead = HeaderSize + eventBytes;

            return new ReplicationLogEntry(tick, evt);
        }
    }
}