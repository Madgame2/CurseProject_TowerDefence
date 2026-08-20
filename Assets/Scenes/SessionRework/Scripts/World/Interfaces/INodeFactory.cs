using Scenes.SessionRework.Scripts.GameWorld.Graph.Interfaces;
using Scenes.SessionRework.Scripts.Services.Sync.DTO.WorldGenerationGraph.Meta;

namespace Scenes.SessionRework.Scripts.GameWorld.Interfaces
{
    public interface INodeFactory
    {
        IGraphNode CreateNode(NodeType type);
    }
}