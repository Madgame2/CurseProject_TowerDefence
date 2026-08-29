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
        public uint UdpToken { get; set; }
        public PacketType Type => PacketType.MoveInput;
        public uint LastTick;
        public MoveInputCommand[] Inputs;

        public int Serialize(Span<byte> buffer)
        {
            int offset = 0;


            BinaryPrimitives.WriteUInt32LittleEndian(buffer.Slice(offset), UdpToken);
            offset += 4;

            buffer[offset] = (byte)Type;
            offset += 1;


            BinaryPrimitives.WriteUInt32LittleEndian(buffer.Slice(offset), LastTick);
            offset += 4;

            int inputsLength = Inputs?.Length ?? 0;
            BinaryPrimitives.WriteInt32LittleEndian(buffer.Slice(offset), inputsLength);
            offset += 4;


            if (Inputs != null)
            {
                for (int i = 0; i < inputsLength; i++)
                {
                    ref readonly var input = ref Inputs[i];
                    float x = input.MoveDirection.x;
                    float y = input.MoveDirection.y;
                    float z = input.MoveDirection.z;

                    BinaryPrimitives.WriteUInt32LittleEndian(buffer.Slice(offset), input.Tick);
                    offset += 4;

                    MemoryMarshal.Write(buffer.Slice(offset), ref x);
                    offset += 4;

                    MemoryMarshal.Write(buffer.Slice(offset), ref y);
                    offset += 4;

                    MemoryMarshal.Write(buffer.Slice(offset), ref z);
                    offset += 4;
                }
            }

            return offset;
        }
    }
}