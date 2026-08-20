using System.Runtime.CompilerServices;
using Scenes.SessionRework.Scripts.GameWorld.Graph.BiomGraph.Meta.Enums;
using Scenes.SessionRework.Scripts.GameWorld.Graph.DecorationsGrpah.Meta.Enum;
using UnityEngine;

namespace Scenes.SessionRework.Scripts.GameWorld.Model
{
    public struct Chunk
    {
        private Vector2Int ChunkCoordinates;
        private readonly float[] _heightMap;
        private readonly BiomeType[] _biomesMap;
        private  readonly DecorationType[] _decorationsMap;
        public int Size { get; }
        public int RawSize => Size+1;
        public Vector2 Pivot { get; }
        public Vector2Int ChunkPos =>  ChunkCoordinates;
        
        public Chunk(Vector2Int chunkCoordinates,Vector2 pivot, int chunkSize)
        {
            ChunkCoordinates = chunkCoordinates;
            Size = chunkSize;
            Pivot = pivot;
            _heightMap = new float[(chunkSize+1) * (chunkSize+1)];
            _biomesMap = new BiomeType[chunkSize * chunkSize];
            _decorationsMap = new  DecorationType[chunkSize * chunkSize];
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetIndex(int x, int y)
        {
            return x + y * RawSize;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetIndex(Vector2Int localCoordinates)
        {
            return localCoordinates.x + localCoordinates.y * RawSize;
        }
        
        public void SetHeight(Vector2Int localCoordinates, float height)
        {
            _heightMap[GetIndex(localCoordinates)] = height;
        }

        public void SetHeight(int x, int y, float height)
        {
            _heightMap[GetIndex(x, y)] = height;
        }
        
        public float GetHeight(Vector2Int localCoordinates)
        {
            return _heightMap[GetIndex(localCoordinates)];
        }
        
        public float GetHeight(int x, int y)
        {
            return _heightMap[GetIndex(x, y)];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetBiomeIndex(int x, int y)
        {
            return x + y * Size; 
        }

        public void SetBiomeInCell(int x, int y, BiomeType biome)
        {
            _biomesMap[GetBiomeIndex(x, y)] = biome;
        }
        
        public void SetBiomeInCell(Vector2Int localCoordinates, BiomeType biome)
        {
            _biomesMap[GetBiomeIndex(localCoordinates.x, localCoordinates.y)] = biome;
        }

        public BiomeType GetBiomeInCell(int x, int y)
        {
            return _biomesMap[GetBiomeIndex(x, y)];
        }
        
        public void SetDecorationInCell(int x, int y, DecorationType decoration)
        {
            _decorationsMap[GetBiomeIndex(x, y)] = decoration;
        }
        
        public void SetDecorationInCell(Vector2Int localCoordinates, DecorationType decoration)
        {
            _decorationsMap[GetBiomeIndex(localCoordinates.x, localCoordinates.y)] = decoration;
        }
        
        public DecorationType GetDecorationInCell(int x, int y)
        {
            return _decorationsMap[GetBiomeIndex(x, y)];
        }

    }
}