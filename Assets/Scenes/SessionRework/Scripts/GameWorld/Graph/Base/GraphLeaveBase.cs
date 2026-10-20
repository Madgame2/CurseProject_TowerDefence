using Scenes.SessionRework.Scripts.GameWorld.Graph.Interfaces;
using Scenes.SessionRework.Scripts.GameWorld.Graph.LandscapeGraph.Interfaces;
using Scenes.SessionRework.Scripts.Services.Sync.DTO.WorldGenerationGraph.Meta;

namespace Scenes.SessionRework.Scripts.GameWorld.Graph.Base
{
    public abstract class GraphLeaveBase : ILandscapeGraphNode
    {
        public abstract IGraphNode[] GetChildren();
        public abstract void Initialize(NodeParam[] parameters, IGraphNode[] allNodes);

        public void AddChild(IGraphNode child)
        {
        }

        public abstract bool TryEvaluate(float x, float y, out float result);
    }
}