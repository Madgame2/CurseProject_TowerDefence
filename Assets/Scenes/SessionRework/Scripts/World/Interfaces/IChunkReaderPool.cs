using UnityEngine;

namespace Scenes.SessionRework.Scripts.World.Interfaces
{
    public interface IChunkReaderPool
    {
        bool HasChunk(Vector2Int chunkCoordinates);
        void Release(Vector2Int chunkCoordinates);
        IChunkReader Get(Vector2Int chunkCoordinates);
    }
}