using Scenes.SessionRework.Scripts.GameWorld.Model;
using UnityEngine;

namespace Scenes.SessionRework.Scripts.GameWorld.Interfaces
{
    public interface IChunkCache
    {
        bool HasChunk(Vector2Int chunkCoordinates);
        bool TryGet(Vector2Int chunkCoordinates, out Chunk chunk);
        void Add(Vector2Int chunkCoordinates, Chunk generatedChunk);
    }
}