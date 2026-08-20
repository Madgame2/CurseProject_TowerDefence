using Scenes.SessionRework.Scripts.GameWorld.Graph.Interfaces;

namespace Scenes.SessionRework.Scripts.GameWorld.Graph.LandscapeGraph.Interfaces
{
    public interface ILandscapeGraphNode: IGraphNode
    {
        float Evaluate(float x, float y);
    }
}