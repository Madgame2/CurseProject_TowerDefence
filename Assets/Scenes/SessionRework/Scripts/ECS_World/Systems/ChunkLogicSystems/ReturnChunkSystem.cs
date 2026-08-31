using Editor.Interfaces.Math;
using Scellecs.Morpeh;
using Scenes.SessionRework.Scripts.ECS_World.Components.Geometry;
using Scenes.SessionRework.Scripts.ECS_World.Components.Players;
using Scenes.SessionRework.Scripts.ECS_World.Components.World;
using Scenes.SessionRework.Scripts.GameWorld.Core.interfaces;
using Scenes.SessionRework.Scripts.GameWorld.Interfaces;
using UnityEngine;

namespace Scenes.SessionRework.Scripts.ECS_World.Systems.ChunkLogicSystems
{
    public class ReturnChunkSystem: ISystem
    {
        public World World { get; set; }
        private readonly IChunkOrchestrator _chunkOrchestrator;
        private readonly IChunksSettings _chunksSettings;
        
        private Stash<MyPlayerComponent> _myPlayerStashStash;
        private Stash<PositionComponent>  _playerPositionStash;
        private Stash<ChunkStorageComponent>   _chunkStorageStash;
        
        private Filter _filter;
        private Filter _storageFilter;

        public ReturnChunkSystem(World world, IChunkOrchestrator chunkOrchestrator, IChunksSettings chunksSettings)
        {
            World = world;
            _chunkOrchestrator= chunkOrchestrator;
            _chunksSettings = chunksSettings;
        }

        public void OnAwake()
        {
            _myPlayerStashStash = World.GetStash<MyPlayerComponent>();
            _playerPositionStash = World.GetStash<PositionComponent>();
            _chunkStorageStash = World.GetStash<ChunkStorageComponent>();

            _filter = World.Filter.With<MyPlayerComponent>().With<PositionComponent>().Build();
            _storageFilter = World.Filter.With<ChunkStorageComponent>().Build();
        }
        
        public void OnUpdate(float deltaTime)
        {
            if (_storageFilter.IsEmpty())
            {
                var chunkStorage =  World.CreateEntity();
                _chunkStorageStash.Set(chunkStorage, new ChunkStorageComponent{ChunkStorage = new()});
            }

            foreach (var storage in _storageFilter)
            {
                foreach (var player in _filter)
                {
                    ref var playerPosition = ref _playerPositionStash.Get(player);
                    ref var chunkStorage = ref  _chunkStorageStash.Get(storage);
                    
                    float xPosition = playerPosition.Position.x;
                    float zPosition = playerPosition.Position.z;
                    int chunkSize = _chunksSettings.ChunkSize;
                    Vector2 chunkPivot = new Vector2(_chunksSettings.Pivot.X, _chunksSettings.Pivot.Y);
                    
                    Vector2Int currentChunkPosition = MathTools.GlobalPosToChunkPosition(xPosition,
                        zPosition,
                        chunkSize,
                        chunkPivot);

                    chunkStorage.ChunkStorage.RemoveWhere(loadedChunk => 
                    {
                        if (loadedChunk != currentChunkPosition)
                        {
                            _chunkOrchestrator.UnloadChunk(loadedChunk);
                            return true;  
                        }

                        return false;
                    });
                }
                break;
            }
        }
        
        public void Dispose()
        {

        }
    }
}