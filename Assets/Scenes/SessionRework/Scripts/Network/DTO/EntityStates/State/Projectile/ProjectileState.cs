using System;
using System.Buffers.Binary;
using UnityEngine;

namespace Scenes.SessionRework.Scripts.Network.DTO.EntityStates.State.Projectile
{
    using System;
using System.Buffers.Binary;
using UnityEngine;

public struct ProjectileState
{
    // uint (4) + Vector3 (3 * 4) + Quaternion (4 * 4)
    public const int ByteSize = sizeof(uint) + 3 * sizeof(float) + 4 * sizeof(float);

    public uint ObjectId;
    public Vector3 Position;
    public Quaternion Rotation;

    public readonly int GetSize() => ByteSize;

    public readonly int WriteTo(Span<byte> destination)
    {
        if (destination.Length < ByteSize)
            throw new ArgumentException(
                $"Буфер слишком мал: нужно {ByteSize} байт, передано {destination.Length}.",
                nameof(destination));

        var offset = 0;

        WriteUInt32(destination, ref offset, ObjectId);
        WriteVector3(destination, ref offset, Position);
        WriteQuaternion(destination, ref offset, Rotation);

        return offset; // == ByteSize
    }

    public int ReadFrom(ReadOnlySpan<byte> source)
    {
        if (source.Length < ByteSize)
            throw new ArgumentException(
                $"Буфер слишком мал: нужно {ByteSize} байт, передано {source.Length}.",
                nameof(source));

        var offset = 0;

        ObjectId = ReadUInt32(source, ref offset);
        Position = ReadVector3(source, ref offset);
        Rotation = ReadQuaternion(source, ref offset);

        return offset; // == ByteSize
    }

    // ---- Write helpers ----

    private static void WriteUInt32(Span<byte> buffer, ref int offset, uint value)
    {
        BinaryPrimitives.WriteUInt32LittleEndian(buffer.Slice(offset, sizeof(uint)), value);
        offset += sizeof(uint);
    }

    private static void WriteSingle(Span<byte> buffer, ref int offset, float value)
    {
        BinaryPrimitives.WriteInt32LittleEndian(
            buffer.Slice(offset, sizeof(float)),
            BitConverter.SingleToInt32Bits(value));
        offset += sizeof(float);
    }

    private static void WriteVector3(Span<byte> buffer, ref int offset, Vector3 v)
    {
        WriteSingle(buffer, ref offset, v.x);
        WriteSingle(buffer, ref offset, v.y);
        WriteSingle(buffer, ref offset, v.z);
    }

    private static void WriteQuaternion(Span<byte> buffer, ref int offset, Quaternion q)
    {
        WriteSingle(buffer, ref offset, q.x);
        WriteSingle(buffer, ref offset, q.y);
        WriteSingle(buffer, ref offset, q.z);
        WriteSingle(buffer, ref offset, q.w);
    }

    // ---- Read helpers ----

    private static uint ReadUInt32(ReadOnlySpan<byte> buffer, ref int offset)
    {
        var value = BinaryPrimitives.ReadUInt32LittleEndian(buffer.Slice(offset, sizeof(uint)));
        offset += sizeof(uint);
        return value;
    }

    private static float ReadSingle(ReadOnlySpan<byte> buffer, ref int offset)
    {
        var bits = BinaryPrimitives.ReadInt32LittleEndian(buffer.Slice(offset, sizeof(float)));
        offset += sizeof(float);
        return BitConverter.Int32BitsToSingle(bits);
    }

    private static Vector3 ReadVector3(ReadOnlySpan<byte> buffer, ref int offset)
    {
        var x = ReadSingle(buffer, ref offset);
        var y = ReadSingle(buffer, ref offset);
        var z = ReadSingle(buffer, ref offset);
        return new Vector3(x, y, z);
    }

    private static Quaternion ReadQuaternion(ReadOnlySpan<byte> buffer, ref int offset)
    {
        var x = ReadSingle(buffer, ref offset);
        var y = ReadSingle(buffer, ref offset);
        var z = ReadSingle(buffer, ref offset);
        var w = ReadSingle(buffer, ref offset);
        return new Quaternion(x, y, z, w);
    }
}
}