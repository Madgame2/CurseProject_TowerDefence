using Scenes.SessionRework.Scripts.GameWorld.Graph.BiomGraph.Meta.Enums;
using Scenes.SessionRework.Scripts.GameWorld.Graph.Interfaces;

namespace Scenes.SessionRework.Scripts.GameWorld.Graph.BiomGraph.Interfaces
{
    public interface IBiomeGraphNode: IGraphNode
    {
        BiomeType Evaluate(float x, float y);
    }
}