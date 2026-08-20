using System;
using Common.Services.Net.Modules;
using Common.systems.SceneStates;
using Scenes.SessionRework.Scripts.Network.Ineterfaces;
using Scenes.SessionRework.Scripts.Services.Sync.DTO;
using Scenes.SessionRework.Scripts.Services.Sync.DTO.PlayersInitDTOs;
using Scenes.SessionRework.Scripts.Services.Sync.DTO.WorldGenerationGraph;
using Scenes.SessionRework.Scripts.Services.Sync.interfaces;
using Zenject;

namespace Scenes.SessionRework.Scripts.Network.Controllers
{
    public class SyncController: ISessionSyncController, IDisposable
    {
        [Inject] private readonly WebSocketModule _webSocket;
        [Inject] private readonly SceneStateMachine<SessionReworkScene> _sceneStateMachine;
        [Inject] private readonly ISyncService _syncService;
        
        public void Dispose()
        {
            _webSocket.Off("PREPARE_FOR_SYNC", goToSyncState);
            _webSocket.Off<ChunkMetaDatasMessage>("METADATA_CHUNK_SETTINGS", handleChunkMetaData);
            _webSocket.Off<WorldGenerationRules>("METADATA_WORLD_GENERATION_SETTINGS", handleWorldGenerationMetaData);
            _webSocket.Off<DecorationRulesMessage>("METADATA_DECORATION_RULES", handleDecorationRulesMessage);
            _webSocket.Off<PlayerInitMessage>("METADATA_PLAYER_INIT", handlePlayerInitMessage);
            _webSocket.Off("METADATA_SYNC_DONE", handleSyncDoneMessage);
        }

        public void SubscribeToServerEvents()
        {
            _webSocket.On("PREPARE_FOR_SYNC", goToSyncState);
            _webSocket.On<ChunkMetaDatasMessage>("METADATA_CHUNK_SETTINGS", handleChunkMetaData);
            _webSocket.On<WorldGenerationRules>("METADATA_WORLD_GENERATION_SETTINGS", handleWorldGenerationMetaData);
            _webSocket.On<DecorationRulesMessage>("METADATA_DECORATION_RULES", handleDecorationRulesMessage);
            _webSocket.On<PlayerInitMessage>("METADATA_PLAYER_INIT", handlePlayerInitMessage);
            _webSocket.On("METADATA_SYNC_DONE", handleSyncDoneMessage);
        }


        private void handleSyncDoneMessage(string obj)
        {
            _syncService.InitWorld();
        }

        private void handlePlayerInitMessage(PlayerInitMessage obj)
        {
            _syncService.ProcessPlayersData(obj);
            
            _ = _webSocket.Send("Applied", null);
        }

        private void handleDecorationRulesMessage(DecorationRulesMessage obj)
        {
            _syncService.ProcessDecorationRules(obj);
            
            _ = _webSocket.Send("Applied", null);
        }

        private void handleWorldGenerationMetaData(WorldGenerationRules obj)
        {
           _syncService.ProcessWorldGenerationRules(obj);

            _ = _webSocket.Send("Applied", null);
        }

        private void handleChunkMetaData(ChunkMetaDatasMessage obj)
        {
            _syncService.SetChunksMetaData(obj);

            _ = _webSocket.Send("Applied", null);
        }

        private void goToSyncState(string arg)
        {
            _sceneStateMachine.tryMoveToState(typeof(SyncState));
        }

        public void SendClientReady()
        {
            _ = _webSocket.Send("ClientReady", null);
        }
    }
}