using System;
using System.Buffers.Binary;
using System.IO;
using System.Runtime.InteropServices;
using Common.Services.Net.Contracts;
using Scenes.SessionRework.Scripts.ECS_World.Components.Addition;
using Scenes.SessionRework.Scripts.ECS_World.Components.Simulation;

namespace Scenes.SessionRework.Scripts.Network.DTO
{

    public struct ClientInputPacket : IUdpPacket
    {
        private const int HeaderSize = 13;
        private const int InputSize =
            sizeof(uint) +       // Tick: 4
            sizeof(float) * 3 +  // MoveDirection: 12
            sizeof(float) * 4 +  // Rotation: 16
            sizeof(float) * 3 +  // LookAtPoint: 12
            sizeof(byte);        // Actions: 1

        public uint UdpToken { get; set; }
        public PacketType Type => PacketType.MoveInput;

        public uint LastTick;
        public PlayerStateSnapShoot[] Inputs;

        public int Serialize(Span<byte> buffer)
        {
            int inputsCount = Inputs?.Length ?? 0;
            int requiredSize = HeaderSize + inputsCount * InputSize;

            if (buffer.Length < requiredSize)
                throw new ArgumentException("Buffer is too small.", nameof(buffer));

            int offset = 0;

            WriteHeader(buffer, ref offset, inputsCount);

            for (int i = 0; i < inputsCount; i++)
            {
                WriteInput(buffer, ref offset, in Inputs[i]);
            }

            return offset;
        }

        private void WriteHeader(
            Span<byte> buffer,
            ref int offset,
            int inputsCount)
        {
            BinaryPrimitives.WriteUInt32LittleEndian(
                buffer.Slice(offset), UdpToken);
            offset += 4;

            buffer[offset++] = (byte)Type;

            BinaryPrimitives.WriteUInt32LittleEndian(
                buffer.Slice(offset), LastTick);
            offset += 4;

            BinaryPrimitives.WriteInt32LittleEndian(
                buffer.Slice(offset), inputsCount);
            offset += 4;
        }

        private static void WriteInput(
            Span<byte> buffer,
            ref int offset,
            in PlayerStateSnapShoot input)
        {
            BinaryPrimitives.WriteUInt32LittleEndian(
                buffer.Slice(offset), input.Tick);
            offset += 4;

            WriteFloat(buffer, ref offset, input.MoveDirection.x);
            WriteFloat(buffer, ref offset, input.MoveDirection.y);
            WriteFloat(buffer, ref offset, input.MoveDirection.z);
            
            WriteFloat(buffer, ref offset, input.Rotation.x);
            WriteFloat(buffer, ref offset, input.Rotation.y);
            WriteFloat(buffer, ref offset, input.Rotation.z);
            WriteFloat(buffer, ref offset, input.Rotation.w);
            
            WriteFloat(buffer, ref offset, input.LookAtPoint.x);
            WriteFloat(buffer, ref offset, input.LookAtPoint.y);
            WriteFloat(buffer, ref offset, input.LookAtPoint.z);

            buffer[offset++] = (byte)input.Actions;
        }

        private static void WriteFloat(
            Span<byte> buffer,
            ref int offset,
            float value)
        {
            int bits = BitConverter.SingleToInt32Bits(value);

            BinaryPrimitives.WriteInt32LittleEndian(
                buffer.Slice(offset), bits);

            offset += sizeof(float);
        }
    }
}