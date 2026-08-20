using System.Threading.Tasks;
using Scenes.SessionRework.Scripts.GameWorld.Graph.BiomGraph.Interfaces;
using Scenes.SessionRework.Scripts.GameWorld.Graph.DecorationsGrpah.Interfaces;
using Scenes.SessionRework.Scripts.GameWorld.Graph.LandscapeGraph.Interfaces;
using Scenes.SessionRework.Scripts.GameWorld.Interfaces;
using Scenes.SessionRework.Scripts.GameWorld.Model;
using Scenes.SessionRework.Scripts.GameWorld.Graph.Interfaces;
using UnityEngine;

namespace Scenes.SessionRework.Scripts.GameWorld.Factories
{
    public class ChunkGenerator : IChunkGenerator
    {
        private readonly ILandscapeGraphNode _landscapeGraph;
        private readonly IBiomeGraphNode _biomeGraph;
        private readonly IDecorationsGraphNode _decorationsGraph;
        private readonly IChunksSettings _chunksSettings;

        public ChunkGenerator(ILandscapeGraphNode landscapeGraph, IBiomeGraphNode biomeGraph,
            IChunksSettings chunksSettings,IDecorationsGraphNode decorationsGraph)
        {
            _landscapeGraph = landscapeGraph;
            _biomeGraph = biomeGraph;
            _chunksSettings = chunksSettings;
            _decorationsGraph = decorationsGraph;
        }

        public async Task<Chunk> GenerateChunkAsync(Vector2Int chunkPos)
        {
            int baseSize = _chunksSettings.ChunkSize;
            int extendedSize = baseSize + 1;

            Vector2 pivot = new Vector2(_chunksSettings.Pivot.X, _chunksSettings.Pivot.Y);

            Chunk chunk = new Chunk(chunkPos, pivot, baseSize);

            Vector2 pivotOffset = pivot * baseSize;

            float chunkWorldOriginX = chunkPos.x * baseSize;
            float chunkWorldOriginY = chunkPos.y * baseSize;

            for (int x = 0; x < extendedSize; x++)
            {
                for (int y = 0; y < extendedSize; y++)
                {
                    float worldX = chunkWorldOriginX + x - pivotOffset.x;
                    float worldY = chunkWorldOriginY + y - pivotOffset.y;

                    Vector2Int localCoordinates = new Vector2Int(x, y);

                    var height = _landscapeGraph.Evaluate(worldX, worldY);

                    chunk.SetHeight(localCoordinates, height);
                }
            }


            for (int x = 0; x < baseSize; x++)
            {
                for (int y = 0; y < baseSize; y++)
                {
                    float worldX = chunkWorldOriginX + x - pivotOffset.x;
                    float worldY = chunkWorldOriginY + y - pivotOffset.y;

                    Vector2Int localCoordinates = new Vector2Int(x, y);

                    var biome = _biomeGraph.Evaluate(worldX, worldY);

                    chunk.SetBiomeInCell(localCoordinates, biome);
                }
            }

            for (int x = 0; x < baseSize; x++)
            {
                for (int y = 0; y < baseSize; y++)
                {
                    float worldX = chunkWorldOriginX + x - pivotOffset.x;
                    float worldY = chunkWorldOriginY + y - pivotOffset.y;

                    Vector2Int localCoordinates = new Vector2Int(x, y);

                    var decorations = _decorationsGraph.Evaluate(worldX, worldY);

                    chunk.SetDecorationInCell(localCoordinates, decorations);
                }
            }

            return chunk;
        }
    }
}