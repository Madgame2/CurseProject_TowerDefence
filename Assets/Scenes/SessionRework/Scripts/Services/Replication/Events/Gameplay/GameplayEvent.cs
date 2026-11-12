using System;
using System.Buffers.Binary;
using Scenes.SessionRework.Scripts.Services.Replication.Events.Gameplay.Payload;
using Scenes.SessionRework.Scripts.Services.Replication.Models;

namespace Scenes.SessionRework.Scripts.Services.Replication.Events.Gameplay
{
    public struct GameplayEvent
    {
        public uint objectId;
        public ObjectType objectType;
        public GameplayeEventPayload payload;

        public int GetSize()
        {
            return 5 + payload.GetSize(objectType);
        }

        public int WriteTo(Span<byte> destination)
        {
            int size = GetSize();

            if (destination.Length < size)
                throw new ArgumentException("Destination buffer is too small.");

            BinaryPrimitives.WriteUInt32LittleEndian(
                destination.Slice(0, 4), objectId);

            destination[4] = (byte)objectType;

            return 5 + payload.WriteTo(
                destination.Slice(5), objectType);
        }
        
        public int ReadFrom(ReadOnlySpan<byte> source)
        {
            if (source.Length < 5)
                throw new ArgumentException("Source buffer is too small.");

            objectId = BinaryPrimitives.ReadUInt32LittleEndian(
                source.Slice(0, 4));

            objectType = (ObjectType)source[4];

            return 5 + payload.ReadFrom(
                source.Slice(5), objectType);
        }
    }
}