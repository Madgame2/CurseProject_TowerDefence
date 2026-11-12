using System;
using System.Runtime.InteropServices;
using Scenes.SessionRework.Scripts.Services.Replication.Events.Gameplay.Payload.Player;
using Scenes.SessionRework.Scripts.Services.Replication.Models;

namespace Scenes.SessionRework.Scripts.Services.Replication.Events.Gameplay.Payload
{
    [StructLayout(LayoutKind.Explicit)]
    public struct GameplayeEventPayload
    {
        [FieldOffset(0)]
        public PlayerGameplayEventPayload PlayerEvent;


        public int GetSize(ObjectType objectType)
        {
            return objectType switch
            {
                ObjectType.Player => PlayerGameplayEventPayload.ByteSize,

                _ => throw new NotSupportedException(
                    $"Unsupported object type: {objectType}")
            };
        }

        public int WriteTo(Span<byte> destination, ObjectType objectType)
        {
            return objectType switch
            {
                ObjectType.Player => PlayerEvent.WriteTo(destination),

                _ => throw new NotSupportedException(
                    $"Unsupported object type: {objectType}")
            };
        }

        public int ReadFrom(ReadOnlySpan<byte> source, ObjectType objectType)
        {
            return objectType switch
            {
                ObjectType.Player => PlayerEvent.ReadFrom(source),

                _ => throw new NotSupportedException(
                    $"Unsupported object type: {objectType}")
            };
        }
    }
}