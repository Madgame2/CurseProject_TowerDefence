using System.Numerics;
using Scenes.SessionRework.Scripts.World.Interfaces;

namespace Scenes.SessionRework.Scripts.World.Model
{
    public class WorldSettings : IChunksSettings
    {
        public int ChunkSize { get; set; }
        public Vector2 Pivot { get; set; }
    }
}