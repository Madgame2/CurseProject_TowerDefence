using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Scenes.SessionRework.Scripts.GameWorld.Core.interfaces
{
    public interface IChunkOrchestrator
    {
        UniTask LoadChunk(Vector2Int chunkCoordinates);
        void UnloadChunk(Vector2Int chunkCoordinates);
        void ForceRebuildChunk(Vector2Int chunkCoordinates);
    }
}
