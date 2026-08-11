using Scenes.SessionRework.Scripts.Services.Sync.DTO.WorldGenerationGraph.Meta;
using Scenes.SessionRework.Scripts.World.Graph.Interfaces;
using Scenes.SessionRework.Scripts.World.Graph.LandscapeGraph.Interfaces;

namespace Scenes.SessionRework.Scripts.World.Graph.Base
{
    public abstract class GraphLeaveBase : ILandscapeGraphNode
    {
        public abstract IGraphNode[] GetChildren();
        public abstract void Initialize(NodeParam[] parameters);

        public void AddChild(IGraphNode child)
        {
        }

        public abstract float Evaluate(float x, float y);
    }
}