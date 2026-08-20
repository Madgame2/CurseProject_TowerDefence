using System.Numerics;
using Scenes.SessionRework.Scripts.GameWorld.Interfaces;

namespace Scenes.SessionRework.Scripts.GameWorld.Model
{
    public class WorldSettings : IChunksSettings
    {
        public int ChunkSize { get; set; }
        public Vector2 Pivot { get; set; }
    }
}