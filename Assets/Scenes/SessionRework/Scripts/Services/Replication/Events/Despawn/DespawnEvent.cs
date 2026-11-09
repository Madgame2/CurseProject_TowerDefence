using System;
using System.Buffers.Binary;

namespace Scenes.SessionRework.Scripts.Services.Replication.Events.Despawn
{
    public struct DespawnEvent
    {
        public const int ByteSize = 4;

        public uint objectId;

        public int WriteTo(Span<byte> destination)
        {
            if (destination.Length < ByteSize)
                throw new ArgumentException("Destination buffer is too small.");

            BinaryPrimitives.WriteUInt32LittleEndian(
                destination.Slice(0, 4), objectId);

            return ByteSize;
        }
        
        public void ReadFrom(ReadOnlySpan<byte> source)
        {
            if (source.Length < ByteSize)
                throw new ArgumentException("Source buffer is too small.");

            objectId = BinaryPrimitives.ReadUInt32LittleEndian(
                source.Slice(0, 4));
        }
    }
}