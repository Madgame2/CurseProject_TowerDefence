using System.Threading.Tasks;
using Scenes.SessionRework.Scripts.World.Model;
using UnityEngine;

namespace Scenes.SessionRework.Scripts.World.Interfaces
{
    public interface IChunkGenerator
    {
        Task<Chunk> GenerateChunkAsync(Vector2Int chunkPos);
    }
}