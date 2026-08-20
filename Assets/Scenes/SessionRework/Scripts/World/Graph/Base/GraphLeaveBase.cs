using Scenes.SessionRework.Scripts.GameWorld.Graph.Interfaces;
using Scenes.SessionRework.Scripts.GameWorld.Graph.LandscapeGraph.Interfaces;
using Scenes.SessionRework.Scripts.Services.Sync.DTO.WorldGenerationGraph.Meta;

namespace Scenes.SessionRework.Scripts.GameWorld.Graph.Base
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