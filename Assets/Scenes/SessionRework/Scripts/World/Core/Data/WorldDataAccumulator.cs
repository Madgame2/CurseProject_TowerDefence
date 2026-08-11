using System;
using Scenes.SessionRework.Scripts.Services.Sync.DTO;
using Scenes.SessionRework.Scripts.Services.Sync.DTO.WorldGenerationGraph;
using Scenes.SessionRework.Scripts.World.Graph.BiomGraph.Interfaces;
using Scenes.SessionRework.Scripts.World.Graph.LandscapeGraph.Interfaces;
using Scenes.SessionRework.Scripts.World.Interfaces;
using Scenes.SessionRework.Scripts.World.Model;

namespace Scenes.SessionRework.Scripts.World.Core.Data
{
    public class WorldDataAccumulator
    {
        private readonly INetGraphsBuilder _netGraphsBuilder;
    
        private IChunksSettings _settings;
        private ILandscapeGraphNode _landscapeGraph;
        private IBiomeGraphNode _biomeGraph;

        public bool IsComplete => _settings != null && _landscapeGraph != null && _biomeGraph != null;

        public WorldDataAccumulator(INetGraphsBuilder netGraphsBuilder)
        {
            _netGraphsBuilder = netGraphsBuilder;
        }

        public void SetMetaData(ChunkMetaDatasMessage message)
        {
            _settings = new WorldSettings
            {
                ChunkSize = message.Size,
                Pivot = message.Pivot
            };
        }

        public void SetGenerationRules(WorldGenerationRules rules)
        {
            var landscapeRoot = _netGraphsBuilder.CreateGrpah(rules.LandscapeGraphDTOs);
            var biomeRoot = _netGraphsBuilder.CreateGrpah(rules.BiomsGraphDTOs);

            if (landscapeRoot is ILandscapeGraphNode landscape && biomeRoot is IBiomeGraphNode biome)
            {
                _landscapeGraph = landscape;
                _biomeGraph = biome;
            }
        }

        // Собирает финальный объект, если всё готово
        public WorldInitializationData Build()
        {
            if (!IsComplete)
                throw new InvalidOperationException("Попытка собрать данные мира до завершения синхронизации!");

            return new WorldInitializationData(_settings, _landscapeGraph, _biomeGraph);
        }
    }
}