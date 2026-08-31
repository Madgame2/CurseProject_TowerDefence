using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Scenes.SessionRework.Scripts.GameWorld.Core.interfaces;
using Scenes.SessionRework.Scripts.GameWorld.Interfaces;
using Scenes.SessionRework.Scripts.GameWorld.Model;
using UnityEngine;

namespace Scenes.SessionRework.Scripts.GameWorld.Core.Orchestrator
{
    public class ChunkOrchestrator:IChunkOrchestrator
    {
        private readonly IChunkReaderPool _chunkReaderPool;
        private readonly IChunkGenerator _chunkGenerator;
        private readonly IChunkCache _chunkCache;

        public ChunkOrchestrator(IChunkReaderPool chunkReaderPool,
            IChunkGenerator chunkGenerator,
            IChunkCache chunkCache)
        {
            _chunkReaderPool = chunkReaderPool;
            _chunkGenerator = chunkGenerator;
            _chunkCache = chunkCache;
        }
        
        public async UniTask LoadChunk(Vector2Int chunkCoordinates)
        {
            if (_chunkCache.HasChunk(chunkCoordinates) && _chunkReaderPool.HasChunk(chunkCoordinates))
            {
                return;
            }
            
            Chunk chunkModel;
            if (!_chunkCache.TryGet(chunkCoordinates, out chunkModel))
            {
                chunkModel = await _chunkGenerator.GenerateChunkAsync(chunkCoordinates);
                _chunkCache.Add(chunkCoordinates, chunkModel);
            }
            
            var chunkReader = _chunkReaderPool.Get(chunkCoordinates);
            chunkReader.Link(ref chunkModel);
        }

        public void UnloadChunk(Vector2Int chunkCoordinates)
        {
            _chunkReaderPool.Release(chunkCoordinates);
        }

        public void ForceRebuildChunk(Vector2Int chunkCoordinates)
        {
            
        }
    }
}