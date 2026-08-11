using Scenes.SessionRework.Scripts.Services.Sync.DTO.WorldGenerationGraph.Meta;
using Scenes.SessionRework.Scripts.World.Graph.Interfaces;

namespace Scenes.SessionRework.Scripts.World.Interfaces
{
    public interface INodeFactory
    {
        IGraphNode CreateNode(NodeType type);
    }
}