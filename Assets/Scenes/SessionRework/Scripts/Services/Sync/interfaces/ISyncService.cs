using Scenes.SessionRework.Scripts.Services.Sync.DTO;
using Scenes.SessionRework.Scripts.Services.Sync.DTO.WorldGenerationGraph;
using Scenes.SessionRework.Scripts.World.Graph.BiomGraph.Interfaces;
using Scenes.SessionRework.Scripts.World.Graph.LandscapeGraph.Interfaces;
using Scenes.SessionRework.Scripts.World.Interfaces;

namespace Scenes.SessionRework.Scripts.Services.Sync.interfaces
{
    public interface ISyncService
    {
        void SetChunksMetaData(ChunkMetaDatasMessage chunkMetaDataMessage);
        void ProcessWorldGenerationRules(WorldGenerationRules worldGenerationRules);
        void InitWorldContainer();
    }
}