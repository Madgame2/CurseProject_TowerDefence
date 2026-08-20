using System.Collections.Generic;
using Scenes.SessionRework.Scripts.GameWorld.Entities;
using Scenes.SessionRework.Scripts.GameWorld.Interfaces;
using Scenes.SessionRework.Scripts.GameWorld.Model;
using UnityEngine;


public class ChunkCache:  IChunkCache
{
    private readonly Dictionary<Vector2Int, Chunk> _chunkCache = new();
    private readonly HashSet<Vector2Int> _chunksInUse = new();
    
    public bool HasChunk(Vector2Int chunkCoordinates)
    {
        return _chunkCache.ContainsKey(chunkCoordinates);
    }

    public bool TryGet(Vector2Int chunkCoordinates, out Chunk chunk)
    {
        if (_chunkCache.TryGetValue(chunkCoordinates, out chunk))
        {
            _chunksInUse.Add(chunkCoordinates);
            return true; 
        }
        
        return false; 
    }

    public void Add(Vector2Int chunkCoordinates, Chunk generatedChunk)
    {
        _chunkCache.Add(chunkCoordinates, generatedChunk);
    }
}