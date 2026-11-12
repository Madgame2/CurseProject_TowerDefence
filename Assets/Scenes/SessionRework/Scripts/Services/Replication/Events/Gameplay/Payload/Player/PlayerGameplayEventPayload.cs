using System;
using System.Buffers.Binary;
using Scenes.SessionRework.Scripts.Services.Replication.Models;

namespace Scenes.SessionRework.Scripts.Services.Replication.Events.Gameplay.Payload.Player
{
    public struct PlayerGameplayEventPayload
    {
        public const int ByteSize = 7;

        public uint PlayerId;
        public PlayerGameplayEventType EventType;
        public ushort ActionId;

        public int WriteTo(Span<byte> destination)
        {
            if (destination.Length < ByteSize)
                throw new ArgumentException("Destination buffer is too small.");

            BinaryPrimitives.WriteUInt32LittleEndian(
                destination.Slice(0, 4), PlayerId);

            destination[4] = (byte)EventType;

            BinaryPrimitives.WriteUInt16LittleEndian(
                destination.Slice(5, 2), ActionId);

            return ByteSize;
        }

        public int ReadFrom(ReadOnlySpan<byte> source)
        {
            if (source.Length < ByteSize)
                throw new ArgumentException("Source buffer is too small.");

            PlayerId = BinaryPrimitives.ReadUInt32LittleEndian(
                source.Slice(0, 4));

            EventType = (PlayerGameplayEventType)source[4];

            ActionId = BinaryPrimitives.ReadUInt16LittleEndian(
                source.Slice(5, 2));

            return ByteSize;
        }
    }
}