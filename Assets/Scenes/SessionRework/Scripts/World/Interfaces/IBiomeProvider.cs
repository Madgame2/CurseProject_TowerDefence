using Scenes.SessionRework.Scripts.GameWorld.Graph.BiomGraph.Meta.Enums;

namespace Scenes.SessionRework.Scripts.GameWorld.Interfaces
{
    public interface IBiomeProvider
    {
        BiomeType GetBiomeGlobal(int globalX, int globalZ);
    }
}
