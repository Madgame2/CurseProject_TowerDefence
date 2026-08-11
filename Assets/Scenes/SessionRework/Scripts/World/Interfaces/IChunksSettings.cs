using System.Numerics;

namespace Scenes.SessionRework.Scripts.World.Interfaces
{
    public interface IChunksSettings
    {
        int ChunkSize { get; }
        public Vector2 Pivot { get; }
    }
}