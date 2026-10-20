using System.Numerics;

namespace Scenes.SessionRework.Scripts.GameWorld.Interfaces
{
    public interface IChunksSettings
    {
        int ChunkSize { get; }
        public Vector2 Pivot { get; }
    }
}