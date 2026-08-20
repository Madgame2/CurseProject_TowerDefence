using Scenes.SessionRework.Scripts.Services.Sync.DTO.WorldGenerationGraph.Meta;

namespace Scenes.SessionRework.Scripts.GameWorld.Graph.Interfaces
{
    public interface IGraphNode
    {
        IGraphNode[] GetChildren();
        void Initialize(NodeParam[] parameters);
        void AddChild(IGraphNode child);
    }
}