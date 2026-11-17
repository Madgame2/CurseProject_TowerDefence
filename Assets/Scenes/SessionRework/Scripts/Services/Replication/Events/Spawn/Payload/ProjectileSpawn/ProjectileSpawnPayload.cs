using System;
using UnityEngine;

namespace Scenes.SessionRework.Scripts.Services.Replication.Events.Spawn.Payload.ProjectileSpawn
{
    using System;
    using System.Buffers.Binary;
    using UnityEngine;

    public struct ProjectileSpawnPayload
    {
        public uint prefabId;
        public Vector3 Position;
        public Quaternion Rotation;

        public const int Size = 32;

        public int WriteTo(Span<byte> destination)
        {
            if (destination.Length < Size)
                throw new ArgumentException("Destination buffer is too small.");

            BinaryPrimitives.WriteUInt32LittleEndian(
                destination,
                prefabId);

            int offset = 4;

            WriteVector3(destination, ref offset, Position);

            WriteQuaternion(destination, ref offset, Rotation);

            return offset;
        }

        public int ReadFrom(ReadOnlySpan<byte> source)
        {
            if (source.Length < Size)
                throw new ArgumentException("Source buffer is too small.");

            prefabId = BinaryPrimitives.ReadUInt32LittleEndian(source);

            int offset = 4;

            Position = ReadVector3(source, ref offset);

            Rotation = ReadQuaternion(source, ref offset);

            return offset;
        }

        private static Vector3 ReadVector3(
            ReadOnlySpan<byte> source,
            ref int offset)
        {
            return new Vector3(
                ReadFloat(source, ref offset),
                ReadFloat(source, ref offset),
                ReadFloat(source, ref offset));
        }

        private static Quaternion ReadQuaternion(
            ReadOnlySpan<byte> source,
            ref int offset)
        {
            return new Quaternion(
                ReadFloat(source, ref offset),
                ReadFloat(source, ref offset),
                ReadFloat(source, ref offset),
                ReadFloat(source, ref offset));
        }

        private static float ReadFloat(
            ReadOnlySpan<byte> source,
            ref int offset)
        {
            float value = BitConverter.Int32BitsToSingle(
                BinaryPrimitives.ReadInt32LittleEndian(
                    source.Slice(offset, 4)));

            offset += 4;

            return value;
        }

        private static void WriteVector3(
            Span<byte> destination,
            ref int offset,
            Vector3 value)
        {
            WriteFloat(destination, ref offset, value.x);
            WriteFloat(destination, ref offset, value.y);
            WriteFloat(destination, ref offset, value.z);
        }

        private static void WriteQuaternion(
            Span<byte> destination,
            ref int offset,
            Quaternion value)
        {
            WriteFloat(destination, ref offset, value.x);
            WriteFloat(destination, ref offset, value.y);
            WriteFloat(destination, ref offset, value.z);
            WriteFloat(destination, ref offset, value.w);
        }

        private static void WriteFloat(
            Span<byte> destination,
            ref int offset,
            float value)
        {
            BinaryPrimitives.WriteInt32LittleEndian(
                destination.Slice(offset, 4),
                BitConverter.SingleToInt32Bits(value));

            offset += 4;
        }
    }
}