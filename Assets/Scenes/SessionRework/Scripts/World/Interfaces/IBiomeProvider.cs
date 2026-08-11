using Scenes.SessionRework.Scripts.World.Graph.BiomGraph.Meta.Enums;

namespace Scenes.SessionRework.Scripts.World.Interfaces
{
    public interface IBiomeProvider
    {
        BiomeType GetBiomeGlobal(int globalX, int globalZ);
    }
}
