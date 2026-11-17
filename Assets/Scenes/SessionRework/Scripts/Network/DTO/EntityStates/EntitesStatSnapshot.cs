using System;
using System.Buffers;
using System.Buffers.Binary;
using Common.Services.Net.Contracts;
using Scenes.SessionRework.Scripts.Network.DTO.EntityStates.State;

namespace Scenes.SessionRework.Scripts.Network.DTO.EntityStates
{
    public struct EntitesStatSnapshot : IServerUdpPaket
    {
        private const int HeaderSize = 6;

        public PacketType Type => PacketType.EntityStateSnapshot;

        public uint ServerTick;

        public int ValidEntitiesCount;

        public EntityState[] entityStates;

        public int Serialize(Span<byte> buffer)
        {
            if (ValidEntitiesCount > ushort.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(ValidEntitiesCount));

            if (ValidEntitiesCount > (entityStates?.Length ?? 0))
                throw new ArgumentException("Invalid entity count.");

            int offset = 0;

            // ServerTick
            BinaryPrimitives.WriteUInt32LittleEndian(
                buffer.Slice(offset, 4), ServerTick);

            offset += 4;

            // EntitiesCount
            BinaryPrimitives.WriteUInt16LittleEndian(
                buffer.Slice(offset, 2), (ushort)ValidEntitiesCount);

            offset += 2;

            // Entities
            for (int i = 0; i < ValidEntitiesCount; i++)
            {
                offset += entityStates[i].WriteTo(buffer.Slice(offset));
            }

            return offset;
        }

        public void Deserialize(ReadOnlySpan<byte> data)
        {
            if (data.Length < HeaderSize)
                throw new ArgumentException("Source buffer is too small.");

            int offset = 0;

            // ServerTick
            ServerTick = BinaryPrimitives.ReadUInt32LittleEndian(
                data.Slice(offset, 4));

            offset += 4;

            // EntitiesCount
            ValidEntitiesCount = BinaryPrimitives.ReadUInt16LittleEndian(
                data.Slice(offset, 2));

            offset += 2;

            // Entities
            entityStates = ValidEntitiesCount == 0
                ? Array.Empty<EntityState>()
                : ArrayPool<EntityState>.Shared.Rent(ValidEntitiesCount);

            try
            {
                for (int i = 0; i < ValidEntitiesCount; i++)
                {
                    offset += entityStates[i].ReadFrom(data.Slice(offset));
                }

                if (offset != data.Length)
                    throw new ArgumentException("Unexpected trailing data.");
            }
            catch
            {
                Release();
                throw;
            }
        }

        public void Release()
        {
            if (entityStates == null)
                return;

            if (ValidEntitiesCount > 0)
            {
                ArrayPool<EntityState>.Shared.Return(
                    entityStates,
                    clearArray: true);
            }

            entityStates = null;
            ValidEntitiesCount = 0;
        }
    }
}