using Scenes.SessionRework.Scripts.GameWorld.Graph.BiomGraph.Interfaces;
using Scenes.SessionRework.Scripts.GameWorld.Graph.BiomGraph.Meta.Enums;
using Scenes.SessionRework.Scripts.GameWorld.Graph.LandscapeGraph.Interfaces;
using Scenes.SessionRework.Scripts.GameWorld.Interfaces;
using Scenes.SessionRework.Scripts.GameWorld.Model;
using UnityEngine;
using Zenject;

namespace Scenes.SessionRework.Scripts.GameWorld.Providers
{
    public class WorldProvider: IWorldProvider
    {
        [Inject] private readonly IChunkCache _chunkCache;
        [Inject] private readonly IBiomeGraphNode _biomeGraph;
        [Inject] private readonly ILandscapeGraphNode _landscapeGraph;
        [Inject] private readonly IChunksSettings _chunksSettings;
        
        public BiomeType GetBiomeGlobal(int globalX, int globalZ)
        {
            int baseSize = _chunksSettings.ChunkSize;
            
            int chunkX = Mathf.FloorToInt((float)globalX / baseSize);
            int chunkY = Mathf.FloorToInt((float)globalZ / baseSize);
        
            Vector2Int chunkPos = new Vector2Int(chunkX, chunkY);
            
            if (_chunkCache.TryGet(chunkPos, out Chunk chunk))
            {
                int localX = globalX - (chunkX * baseSize);
                int localZ = globalZ - (chunkY * baseSize);
            
                return chunk.GetBiomeInCell(localX, localZ);
            }
            
            return _biomeGraph.Evaluate(globalX, globalZ);
        }
    }
}