using System.Threading.Tasks;
using Scenes.SessionRework.Scripts.GameWorld.Model;
using UnityEngine;

namespace Scenes.SessionRework.Scripts.GameWorld.Interfaces
{
    public interface IChunkGenerator
    {
        Task<Chunk> GenerateChunkAsync(Vector2Int chunkPos);
    }
}