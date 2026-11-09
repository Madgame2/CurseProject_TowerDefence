using System;
using System.Buffers.Binary;
using Scenes.SessionRework.Scripts.Services.Replication.Models;

namespace Scenes.SessionRework.Scripts.Services.Replication.Events.Spawn.Payload.PlayerSpawn
{
    public struct PlayerSpawnPayload
    {
        public const int ByteSize = 4 + TransformSnapShoot.ByteSize;

        public uint playerID;
        public TransformSnapShoot Transforms;

        public int WriteTo(Span<byte> destination)
        {
            if (destination.Length < ByteSize)
                throw new ArgumentException("Destination buffer is too small.");

            BinaryPrimitives.WriteUInt32LittleEndian(
                destination.Slice(0, 4), playerID);

            return 4 + Transforms.WriteTo(destination.Slice(4));
        }
        
        public int ReadFrom(ReadOnlySpan<byte> source)
        {
            if (source.Length < ByteSize)
                throw new ArgumentException("Source buffer is too small.");

            playerID = BinaryPrimitives.ReadUInt32LittleEndian(
                source.Slice(0, 4));

            return 4 + Transforms.ReadFrom(source.Slice(4));
        }
    }
}