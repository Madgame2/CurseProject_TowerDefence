using System;
using System.Buffers;
using System.Buffers.Binary;
using System.IO;
using Common.Services.Net.Contracts;

namespace Scenes.SessionRework.Scripts.Network.DTO
{
    public struct ReplicationLogPaket : IServerUdpPaket
    {
        private const int HeaderSize = 6;

        public PacketType Type => PacketType.ReplicationLog;

        public uint ServerTick;
        public int ValidEntriesCount;
        public ReplicationLogEntry[] Entries;

        public int Serialize(Span<byte> buffer)
        {
            int offset = 0;

            // ServerTick
            BinaryPrimitives.WriteUInt32LittleEndian(
                buffer.Slice(offset, 4), ServerTick);

            offset += 4;

            // EntriesCount
            BinaryPrimitives.WriteUInt16LittleEndian(
                buffer.Slice(offset, 2), (ushort)ValidEntriesCount);

            offset += 2;

            // Entries
            for (int i = 0; i < ValidEntriesCount; i++)
            {
                offset += Entries[i].WriteTo(buffer.Slice(offset));
            }

            return offset;
        }

        public void Deserialize(ReadOnlySpan<byte> data)
        {
            int offset = 0;

            // ServerTick
            ServerTick = BinaryPrimitives.ReadUInt32LittleEndian(
                data.Slice(offset, 4));

            offset += 4;

            // EntriesCount
            ValidEntriesCount = BinaryPrimitives.ReadUInt16LittleEndian(
                data.Slice(offset, 2));

            offset += 2;

            // Entries
            Entries = ArrayPool<ReplicationLogEntry>.Shared.Rent(
                ValidEntriesCount);

            for (int i = 0; i < ValidEntriesCount; i++)
            {
                Entries[i] = ReplicationLogEntry.ReadFrom(
                    data.Slice(offset),
                    out int bytesRead);

                offset += bytesRead;
            }
        }

        public void Release()
        {
            if (Entries == null)
                return;

            ArrayPool<ReplicationLogEntry>.Shared.Return(
                Entries,
                clearArray: true);

            Entries = null;
            ValidEntriesCount = 0;
        }
    }
}