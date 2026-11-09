using System;
using System.Buffers.Binary;
using Scenes.SessionRework.Scripts.Services.Replication.Events.Spawn.Payload;
using Scenes.SessionRework.Scripts.Services.Replication.Events.Spawn.Payload.PlayerSpawn;
using Scenes.SessionRework.Scripts.Services.Replication.Models;

namespace Scenes.SessionRework.Scripts.Services.Replication.Events.Spawn
{
    public struct SpawnEvent
    {
        public const int HeaderSize = 5;

        public uint objectId;
        public ObjectType objectType;
        public SpawnPayload payload;

        public int GetSize()
        {
            return HeaderSize + PlayerSpawnPayload.ByteSize;
        }

        public int WriteTo(Span<byte> destination)
        {
            int size = GetSize();

            if (destination.Length < size)
                throw new ArgumentException("Destination buffer is too small.");

            BinaryPrimitives.WriteUInt32LittleEndian(
                destination.Slice(0, 4), objectId);

            destination[4] = (byte)objectType;

            return HeaderSize +
                   payload.WriteTo(destination.Slice(HeaderSize), objectType);
        }
        
        public int ReadFrom(ReadOnlySpan<byte> source)
        {
            if (source.Length < HeaderSize)
                throw new ArgumentException("Source buffer is too small.");

            objectId = BinaryPrimitives.ReadUInt32LittleEndian(
                source.Slice(0, 4));

            objectType = (ObjectType)source[4];

            return HeaderSize +
                   payload.ReadFrom(source.Slice(HeaderSize), objectType);
        }
    }
}