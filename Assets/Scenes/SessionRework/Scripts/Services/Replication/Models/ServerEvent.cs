using System;
using System.Runtime.InteropServices;
using Scenes.SessionRework.Scripts.Network.Meta.Enums;
using Scenes.SessionRework.Scripts.Services.Replication.Events;
using Scenes.SessionRework.Scripts.Services.Replication.Events.Despawn;
using Scenes.SessionRework.Scripts.Services.Replication.Events.Spawn;
using Scenes.SessionRework.Scripts.Services.Replication.Events.Spawn.Payload;

namespace Scenes.SessionRework.Scripts.Services.Replication.Models
{
    [StructLayout(LayoutKind.Explicit)]
    public struct ServerEvent
    {
        [FieldOffset(0)]
        public EventType eventType;

        [FieldOffset(1)]
        public SpawnEvent spawnEvent;

        [FieldOffset(1)]
        public DespawnEvent despawnEvent;

        public int GetSize()
        {
            return eventType switch
            {
                EventType.SpawnEvent => 1 + spawnEvent.GetSize(),
                EventType.DespawnEvent => 1 + DespawnEvent.ByteSize,
                _ => throw new NotSupportedException()
            };
        }

        public int WriteTo(Span<byte> destination)
        {
            int size = GetSize();

            if (destination.Length < size)
                throw new ArgumentException("Destination buffer is too small.");

            destination[0] = (byte)eventType;

            int written = eventType switch
            {
                EventType.SpawnEvent =>
                    spawnEvent.WriteTo(destination.Slice(1)),

                EventType.DespawnEvent =>
                    despawnEvent.WriteTo(destination.Slice(1)),

                _ => throw new NotSupportedException()
            };

            return 1 + written;
        }
        
        public int ReadFrom(ReadOnlySpan<byte> source)
        {
            int offset = 0;

            if (source.Length < 1)
                throw new ArgumentException("Source buffer is too small.");

            eventType = (EventType)source[offset];
            offset++;

            switch (eventType)
            {
                case EventType.SpawnEvent:
                    offset += spawnEvent.ReadFrom(source.Slice(offset));
                    break;

                case EventType.DespawnEvent:
                    despawnEvent.ReadFrom(source.Slice(offset));
                    offset += DespawnEvent.ByteSize;
                    break;

                default:
                    throw new NotSupportedException(
                        $"Unsupported event type: {eventType}");
            }

            return offset;
        }

        public static ServerEvent Created(uint objectId, ObjectType objectType, SpawnPayload payload)
        {
            return new ServerEvent
            {
                eventType = EventType.SpawnEvent,
                spawnEvent = new SpawnEvent
                {
                    objectId = objectId,
                    objectType = objectType,
                    payload = payload
                }
            };
        }

        public static ServerEvent Removed()
        {
            return new ServerEvent
            {
                eventType = EventType.DespawnEvent,
                despawnEvent = new DespawnEvent
                {

                }
            };
        }
    }
}