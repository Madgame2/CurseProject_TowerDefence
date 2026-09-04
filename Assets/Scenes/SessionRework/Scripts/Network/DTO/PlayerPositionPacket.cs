using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Common.Services.Net.Contracts;
using UnityEngine;

namespace Scenes.SessionRework.Scripts.Network.DTO
{
    public struct PlayerStateSnapshot : IServerUdpPaket
    {
        public PacketType Type => PacketType.PlayerWorldState;
        public uint ServerTick;
        public int ValidPlayersCount;
        public PlayerState[] PlayersState;
        

        public int Serialize(Span<byte> buffer)
        {
            int offset = 0;

            buffer[offset] = (byte)Type;
            offset += 1;

            uint tick = ServerTick;
            MemoryMarshal.Write(buffer.Slice(offset), ref tick);
            offset += 4;

            ushort playersCount = (ushort)PlayersState.Length;
            MemoryMarshal.Write(buffer.Slice(offset), ref playersCount);
            offset += 2;

            foreach (var playerState in PlayersState)
            {
                offset += playerState.Serialize(buffer.Slice(offset));
            }

            return offset;
        }

        public void Deserialize(ReadOnlySpan<byte> data)
        {
            int offset = 0;
        
            ServerTick = MemoryMarshal.Read<uint>(data.Slice(offset));
            offset += 4;

            ValidPlayersCount = MemoryMarshal.Read<ushort>(data.Slice(offset));
            offset += 2;
            
            PlayersState = System.Buffers.ArrayPool<PlayerState>.Shared.Rent(ValidPlayersCount);

            for (int i = 0; i < ValidPlayersCount; i++)
            {
                offset += PlayersState[i].Deserialize(data.Slice(offset));
            }
        }

        public void Release()
        {
            if (PlayersState != null)
            {
                System.Buffers.ArrayPool<PlayerState>.Shared.Return(PlayersState);
                PlayersState = null;
            }
        }
    }
}