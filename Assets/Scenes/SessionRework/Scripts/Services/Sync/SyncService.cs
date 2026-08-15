using System;
using System.IO;
using System.Xml;
using Installers;
using Scenes.SessionRework.Scripts.Services.Sync.DTO;
using Scenes.SessionRework.Scripts.Services.Sync.DTO.WorldGenerationGraph;
using Scenes.SessionRework.Scripts.Services.Sync.interfaces;
using Scenes.SessionRework.Scripts.World.Core;
using Scenes.SessionRework.Scripts.World.Core.Data;
using Scenes.SessionRework.Scripts.World.Core.interfaces.BackendParams;
using Scenes.SessionRework.Scripts.World.Entities;
using Scenes.SessionRework.Scripts.World.Graph.BiomGraph.Interfaces;
using Scenes.SessionRework.Scripts.World.Graph.LandscapeGraph.Interfaces;
using Scenes.SessionRework.Scripts.World.Interfaces;
using Scenes.SessionRework.Scripts.World.Model;
using UnityEngine;
using Zenject;
using System.Xml.Linq;
using System.Xml.Serialization;
using JetBrains.Annotations;
using Scenes.SessionRework.Scripts.World.Entities.Chunk;
using Scenes.SessionRework.Scripts.World.Graph.DecorationsGrpah.DecorationRulesModels;

namespace Scenes.SessionRework.Scripts.Services.Sync
{
    public class SyncService: ISyncService
    {
        [Inject] private readonly DiContainer _container;
        [Inject] private readonly INetGraphsBuilder _netGraphsBuilder;
        [Inject] private readonly IDecorationGraphBuilder _decorationGraphBuilder;
        [Inject] private readonly ISetBackendParam _setBackendParam;
        [Inject] private readonly IGetBackendParam _getBackendParam;
        [Inject] private readonly ChunkReader _chunkReaderPrefab;
        
        [Inject] private readonly WorldHolder _worldContainer;
        
        public void OnPacketReceived()
        {

        }
        
        public void SetChunksMetaData(ChunkMetaDatasMessage chunkMetaDataMessage)
        {
            var settings = new WorldSettings
            {
                ChunkSize = chunkMetaDataMessage.Size,
                Pivot = chunkMetaDataMessage.Pivot
            };
            
            _setBackendParam.ChunksSettings = settings;
            
            _container.Rebind<IChunksSettings>().FromInstance(settings).AsSingle();
            
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

        public void InitWorldContainer()
        {
            if (_getBackendParam.ChunksSettings == null ||
                _getBackendParam.LandscapeGraphRoot == null ||
                _getBackendParam.BiomeGraphRoot == null)
            {
                Debug.LogError("Попытка инициализировать мир до получения всех данных!");
                return;
            }
            
            var worldContainer = _container.CreateSubContainer();
            var pendingData = new WorldInitializationData(
                _getBackendParam.ChunksSettings,
                _getBackendParam.LandscapeGraphRoot,
                _getBackendParam.BiomeGraphRoot,
                _getBackendParam.DecorationsGraphRoot);
            
            WorldInstaller.Install(worldContainer,pendingData , _chunkReaderPrefab);
            
            _worldContainer.SetWorldContainer(worldContainer);
            
            Debug.Log("Контекст мира успешно создан и проинициализирован!");
        }

        public void ProcessDecorationRules(DecorationRulesMessage decorationRulesMessage)
        {
            var rulesObject = ParseDecorationRules(decorationRulesMessage);

            if (rulesObject == null)
            {
                Debug.LogError("Decoration rules parse error: rulesObject == null");
                return;
            }
            
            var decorationsGraphRoot = _decorationGraphBuilder.CreateGraph(rulesObject);
            
            _setBackendParam.DecorationsGraphRoot = decorationsGraphRoot;
            
            Debug.Log("Правила декораций получены!");
            
            InitWorldContainer();
        }

        private DecorationRules? ParseDecorationRules(DecorationRulesMessage decorationRulesMessage)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(DecorationRules));
            using (var stream = new MemoryStream(decorationRulesMessage.XmlPayload))
            {
                DecorationRules? rules = serializer.Deserialize(stream) as DecorationRules;
                    
                return rules;
            }
        }
    }
}