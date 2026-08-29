using System;
using Scenes.SessionRework.Scripts.Services.Sync.DTO;
using Scenes.SessionRework.Scripts.Services.Sync.DTO.WorldGenerationGraph;
using Scenes.SessionRework.Scripts.Services.Sync.interfaces;
using Scenes.SessionRework.Scripts.GameWorld.Core.interfaces.BackendParams;
using Scenes.SessionRework.Scripts.GameWorld.Graph.BiomGraph.Interfaces;
using Scenes.SessionRework.Scripts.GameWorld.Graph.LandscapeGraph.Interfaces;
using Scenes.SessionRework.Scripts.GameWorld.Interfaces;
using Scenes.SessionRework.Scripts.GameWorld.Model;
using UnityEngine;
using Zenject;
using Scenes.SessionRework.Scripts.Common.Installers;
using Scenes.SessionRework.Scripts.Network.Parsers.Interfaces;

namespace Scenes.SessionRework.Scripts.Services.Sync
{
    public class SyncService: ISyncService
    {
        [Inject] private readonly INetGraphsBuilder _netGraphsBuilder;
        [Inject] private readonly IDecorationGraphBuilder _decorationGraphBuilder;
        [Inject] private readonly ISetBackendParam _setBackendParam;
        [Inject] private readonly IDecorationRulesParser _decorationRulesParser;
        [Inject] private readonly IWorldInitializer _worldInitializer;
        
        public void SetChunksMetaData(ChunkMetaDatasMessage chunkMetaDataMessage)
        {
            var settings = new WorldSettings
            {
                ChunkSize = chunkMetaDataMessage.Size,
                Pivot = chunkMetaDataMessage.Pivot
            };
            
            _setBackendParam.ChunksSettings = settings;
            
            Debug.Log("IChunksSettings динамически зарегистрирован!");
        }

        public void ProcessWorldGenerationRules(WorldGenerationRules worldGenerationRules)
        {
            var landscape_graphRoot = _netGraphsBuilder.CreateGrpah(worldGenerationRules.LandscapeGraphDTOs);
            var biome_GraphRoot = _netGraphsBuilder.CreateGrpah(worldGenerationRules.BiomsGraphDTOs);

            if (landscape_graphRoot is not ILandscapeGraphNode landscapeGraph ||
                biome_GraphRoot is not IBiomeGraphNode biomeGraph)
            {
                throw new ArgumentException("Один из графов имеет неверный тип.");
            }
            
            _setBackendParam.LandscapeGraphRoot = landscapeGraph;
            _setBackendParam.BiomeGraphRoot = biomeGraph;
        }

        public void InitWorld()
        {
            _worldInitializer.Initialize();
        }

        public void ProcessDecorationRules(
            DecorationRulesMessage decorationRulesMessage)
        {
            var rulesObject = _decorationRulesParser.Parse(decorationRulesMessage.XmlPayload);

            if (rulesObject == null)
            {
                Debug.LogError("Decoration rules parse error.");
                return;
            }

            var decorationsGraphRoot = _decorationGraphBuilder.CreateGraph(rulesObject);

            _setBackendParam.DecorationsGraphRoot = decorationsGraphRoot;
        }

        public void ProcessPlayersData(PlayerInitMessage playerInitMessage)
        {
            _setBackendParam.PlayersArray = playerInitMessage.Players;
        }

        public void UpdateUdpToken(uint objUpdToken)
        {
            _setBackendParam.UdpToken = objUpdToken;
        }
    }
}