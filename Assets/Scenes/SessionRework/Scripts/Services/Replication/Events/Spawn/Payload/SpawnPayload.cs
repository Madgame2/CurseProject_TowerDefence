using System;
using System.Runtime.InteropServices;
using Scenes.SessionRework.Scripts.Services.Replication.Events.Spawn.Payload.PlayerSpawn;
using Scenes.SessionRework.Scripts.Services.Replication.Events.Spawn.Payload.ProjectileSpawn;
using Scenes.SessionRework.Scripts.Services.Replication.Models;

namespace Scenes.SessionRework.Scripts.Services.Replication.Events.Spawn.Payload
{
    [StructLayout(LayoutKind.Explicit)]
    public struct SpawnPayload
    {
        public const int ByteSize = PlayerSpawnPayload.ByteSize;

        [FieldOffset(0)]
        public PlayerSpawnPayload playerPayload;

        [FieldOffset(0)]
        public ProjectileSpawnPayload projectilePayload;

        public int WriteTo(Span<byte> destination, ObjectType objectType)
        {
            return objectType switch
            {
                ObjectType.Player => playerPayload.WriteTo(destination),
                ObjectType.Projectile => projectilePayload.WriteTo(destination),
                _ => throw new NotSupportedException(
                    $"Unsupported object type: {objectType}")
            };
        }
        
        public int ReadFrom(ReadOnlySpan<byte> source, ObjectType objectType)
        {
            return objectType switch
            {
                ObjectType.Player => playerPayload.ReadFrom(source),
                ObjectType.Projectile => projectilePayload.ReadFrom(source),

                _ => throw new NotSupportedException(
                    $"Unsupported object type: {objectType}")
            };
        }
    }
}