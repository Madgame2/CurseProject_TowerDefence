using System;
using System.Buffers.Binary;
using UnityEngine;

namespace Scenes.SessionRework.Scripts.Services.Replication.Models
{
    public struct TransformSnapShoot
    {
        public const int ByteSize = 40;

        public Vector3 Position;
        public Quaternion Rotation;
        public Vector3 Scale;

        public int WriteTo(Span<byte> destination)
        {
            if (destination.Length < ByteSize)
                throw new ArgumentException("Destination buffer is too small.");

            int offset = 0;

            WriteVector3(destination, ref offset, Position);
            WriteQuaternion(destination, ref offset, Rotation);
            WriteVector3(destination, ref offset, Scale);

            return offset;
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
        
        
        public int ReadFrom(ReadOnlySpan<byte> source)
        {
            if (source.Length < ByteSize)
                throw new ArgumentException("Source buffer is too small.");

            int offset = 0;

            Position = ReadVector3(source, ref offset);
            Rotation = ReadQuaternion(source, ref offset);
            Scale = ReadVector3(source, ref offset);

            return offset;
        }

        private static Vector3 ReadVector3(
            ReadOnlySpan<byte> source,
            ref int offset)
        {
            float x = ReadFloat(source, ref offset);
            float y = ReadFloat(source, ref offset);
            float z = ReadFloat(source, ref offset);

            return new Vector3(x, y, z);
        }

        private static Quaternion ReadQuaternion(
            ReadOnlySpan<byte> source,
            ref int offset)
        {
            float x = ReadFloat(source, ref offset);
            float y = ReadFloat(source, ref offset);
            float z = ReadFloat(source, ref offset);
            float w = ReadFloat(source, ref offset);

            return new Quaternion(x, y, z, w);
        }

        private static float ReadFloat(
            ReadOnlySpan<byte> source,
            ref int offset)
        {
            int bits = BinaryPrimitives.ReadInt32LittleEndian(
                source.Slice(offset, 4));

            offset += 4;

            return BitConverter.Int32BitsToSingle(bits);
        }
    }
}