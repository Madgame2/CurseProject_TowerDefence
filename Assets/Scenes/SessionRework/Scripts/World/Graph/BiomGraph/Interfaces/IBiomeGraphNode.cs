using Scenes.SessionRework.Scripts.World.Graph.BiomGraph.Meta.Enums;
using Scenes.SessionRework.Scripts.World.Graph.Interfaces;

namespace Scenes.SessionRework.Scripts.World.Graph.BiomGraph.Interfaces
{
    public interface IBiomeGraphNode: IGraphNode
    {
        BiomeType Evaluate(float x, float y);
    }
}