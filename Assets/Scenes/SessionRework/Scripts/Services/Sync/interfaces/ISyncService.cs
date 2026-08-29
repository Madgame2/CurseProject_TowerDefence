using Scenes.SessionRework.Scripts.Services.Sync.DTO;
using Scenes.SessionRework.Scripts.Services.Sync.DTO.WorldGenerationGraph;
using Scenes.SessionRework.Scripts.GameWorld.Graph.BiomGraph.Interfaces;
using Scenes.SessionRework.Scripts.GameWorld.Graph.LandscapeGraph.Interfaces;
using Scenes.SessionRework.Scripts.GameWorld.Interfaces;

namespace Scenes.SessionRework.Scripts.Services.Sync.interfaces
{
    public interface ISyncService
    {
        void SetChunksMetaData(ChunkMetaDatasMessage chunkMetaDataMessage);
        void ProcessWorldGenerationRules(WorldGenerationRules worldGenerationRules);
        void InitWorld();
        void ProcessDecorationRules(DecorationRulesMessage decorationRulesMessage);
        void ProcessPlayersData(PlayerInitMessage playerInitMessage);
        void UpdateUdpToken(uint objUpdToken);
    }
}