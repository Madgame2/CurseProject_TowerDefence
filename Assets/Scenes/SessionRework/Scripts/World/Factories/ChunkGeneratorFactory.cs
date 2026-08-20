using Scenes.SessionRework.Scripts.GameWorld.Graph.BiomGraph.Interfaces;
using Scenes.SessionRework.Scripts.GameWorld.Graph.DecorationsGrpah.Interfaces;
using Scenes.SessionRework.Scripts.GameWorld.Graph.LandscapeGraph.Interfaces;
using Scenes.SessionRework.Scripts.GameWorld.Interfaces;
using Scenes.SessionRework.Scripts.GameWorld.Graph.Interfaces;
using Zenject;

namespace Scenes.SessionRework.Scripts.GameWorld.Factories
{
    public class ChunkGeneratorFactory: IChunkGeneratorFactory
    {
        [Inject] private readonly DiContainer _container;
        
        public IChunkGenerator Create(ILandscapeGraphNode landscapeGraph, IBiomeGraphNode biomeGraph,IDecorationsGraphNode decorationsGraph)
        {
            var chunkSettings = _container.TryResolve<IChunksSettings>();
            return new ChunkGenerator(landscapeGraph,biomeGraph,chunkSettings,decorationsGraph);
        }
    }
}