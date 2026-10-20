using System.Collections.Generic;
using Scenes.SessionRework.Scripts.GameWorld.Entities.Chunk;
using Scenes.SessionRework.Scripts.GameWorld.Interfaces;
using Scenes.SessionRework.Scripts.GameWorld.Entities;
using UnityEngine;
using Zenject;

namespace Scenes.SessionRework.Scripts.GameWorld.Pool
{
    public class ChunkReaderPool: IChunkReaderPool, IInitializable
    {
        private readonly DiContainer _container;
        private readonly ChunkReader _prefab;
        private readonly Transform _poolContainer;
        
        private IChunksSettings _chunksSettings;
        
        private readonly Dictionary<Vector2Int, IChunkReader> _activeReaders = new();
        private readonly Queue<IChunkReader> _inactiveReaders = new();
        
        public ChunkReaderPool(ChunkReader prefab, DiContainer container)
        {
            _container = container;
            _prefab = prefab;
            
            var containerGo = new GameObject("[ChunkReaderPool]");
            
            _poolContainer = containerGo.transform;
        }
        
        void IInitializable.Initialize()
        {
            for (int i = 0; i < 10; i++)
            {
                var reader = CreateNewReader();
                reader.GameObject.SetActive(false);
                _inactiveReaders.Enqueue(reader);
            }
        }
        
        public bool HasChunk(Vector2Int chunkCoordinates)
        {
            return _activeReaders.ContainsKey(chunkCoordinates);
        }

        public IChunkReader Get(Vector2Int chunkCoordinates)
        {
            if (_activeReaders.TryGetValue(chunkCoordinates, out var activeReader))
            {
                return activeReader;
            }

            if (_chunksSettings == null)
            {
                _chunksSettings = _container.TryResolve<IChunksSettings>();
            }
            
            IChunkReader reader = _inactiveReaders.Count > 0 
                ? _inactiveReaders.Dequeue() 
                : CreateNewReader();
            
            _activeReaders.Add(chunkCoordinates, reader);
            
            Vector3 worldPosition = new Vector3(
                chunkCoordinates.x * _chunksSettings.ChunkSize, 
                0, 
                chunkCoordinates.y * _chunksSettings.ChunkSize
            );
            
            reader.Transform.position = worldPosition;
            reader.GameObject.SetActive(true);
            
            return reader;
        }
        
        public void Release(Vector2Int chunkCoordinates)
        {
            if (_activeReaders.Remove(chunkCoordinates, out var reader))
            {
                reader.Clear(); 
            
                reader.GameObject.SetActive(false);
                _inactiveReaders.Enqueue(reader);
            }
        }
        
        private IChunkReader CreateNewReader()
        {
            return _container.InstantiatePrefabForComponent<IChunkReader>(_prefab, _poolContainer);        }
    }
}