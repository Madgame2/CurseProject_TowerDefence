using System;
using System.Runtime.InteropServices;
using Scenes.SessionRework.Scripts.Network.DTO.EntityStates.State.Projectile;
using Scenes.SessionRework.Scripts.Services.Replication.Models;
using UnityEngine;

namespace Scenes.SessionRework.Scripts.Network.DTO.EntityStates.State
{
    [StructLayout(LayoutKind.Explicit)]
    public struct EntityState
    {
        [FieldOffset(0)]
        public ObjectType objectType;

        [FieldOffset(1)]
        public ProjectileState projectile;

        public readonly int GetSize()
        {
            return 1 + (objectType switch
            {
                ObjectType.Projectile => projectile.GetSize(),
                _ => throw new NotSupportedException(
                    $"Unsupported entity type: {objectType}")
            });
        }

        public readonly int WriteTo(Span<byte> destination)
        {
            int size = GetSize();

            if (destination.Length < size)
                throw new ArgumentException("Destination buffer is too small.");

            destination[0] = (byte)objectType;

            return 1 + (objectType switch
            {
                ObjectType.Projectile => projectile.WriteTo(destination.Slice(1)),
                _ => throw new NotSupportedException(
                    $"Unsupported entity type: {objectType}")
            });
        }

        public int ReadFrom(ReadOnlySpan<byte> source)
        {
            if (source.Length < 1)
                throw new ArgumentException("Source buffer is too small.");

            objectType = (ObjectType)source[0];

            return 1 + (objectType switch
            {
                ObjectType.Projectile => projectile.ReadFrom(source.Slice(1)),
                _ => throw new NotSupportedException(
                    $"Unsupported entity type: {objectType}")
            });
        }

        public static EntityState ProjectileSnapshot(
            uint objectID,
            Vector3 position,
            Quaternion rotation)
        {
            return new EntityState
            {
                objectType = ObjectType.Projectile,
                projectile = new ProjectileState
                {
                    ObjectId = objectID,
                    Position = position,
                    Rotation = rotation
                }
            };
        }
    }
}