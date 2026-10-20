using Scenes.SessionRework.Scripts.Services.Sync.DTO.WorldGenerationGraph.Meta;

namespace Scenes.SessionRework.Scripts.GameWorld.Graph.Interfaces
{
    public interface IGraphNode
    {
        IGraphNode[] GetChildren();
        void Initialize(NodeParam[] parameters, IGraphNode[] allNodes);
        void AddChild(IGraphNode child);
    }
    
    public interface IGraphNode<TOutput> : IGraphNode 
    {
        bool TryEvaluate(float x, float y, out TOutput  output);
    }
}