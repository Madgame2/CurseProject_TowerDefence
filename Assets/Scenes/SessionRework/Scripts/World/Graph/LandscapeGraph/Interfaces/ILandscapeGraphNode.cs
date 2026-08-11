using Scenes.SessionRework.Scripts.World.Graph.Interfaces;

namespace Scenes.SessionRework.Scripts.World.Graph.LandscapeGraph.Interfaces
{
    public interface ILandscapeGraphNode: IGraphNode
    {
        float Evaluate(float x, float y);
    }
}