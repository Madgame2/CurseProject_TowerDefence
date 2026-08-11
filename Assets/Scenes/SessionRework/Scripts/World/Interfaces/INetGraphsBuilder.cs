using Scenes.SessionRework.Scripts.Services.Sync.DTO.WorldGenerationGraph;
using Scenes.SessionRework.Scripts.Services.Sync.DTO.WorldGenerationGraph.Meta;
using Scenes.SessionRework.Scripts.World.Graph.Interfaces;

namespace Scenes.SessionRework.Scripts.World.Interfaces
{
    public interface INetGraphsBuilder
    {
        public IGraphNode CreateGrpah(GraphNodeDTO[] GraphDTOs);
    }
}