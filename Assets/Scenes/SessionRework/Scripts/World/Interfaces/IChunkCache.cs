using Scenes.SessionRework.Scripts.World.Model;
using UnityEngine;

namespace Scenes.SessionRework.Scripts.World.Interfaces
{
    public interface IChunkCache
    {
        bool HasChunk(Vector2Int chunkCoordinates);
        bool TryGet(Vector2Int chunkCoordinates, out Chunk chunk);
        void Add(Vector2Int chunkCoordinates, Chunk generatedChunk);
    }
}