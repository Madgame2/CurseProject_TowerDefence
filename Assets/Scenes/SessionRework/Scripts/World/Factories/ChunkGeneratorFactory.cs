using Scenes.SessionRework.Scripts.World.Graph.BiomGraph.Interfaces;
using Scenes.SessionRework.Scripts.World.Graph.DecorationsGrpah.Interfaces;
using Scenes.SessionRework.Scripts.World.Graph.Interfaces;
using Scenes.SessionRework.Scripts.World.Graph.LandscapeGraph.Interfaces;
using Scenes.SessionRework.Scripts.World.Interfaces;
using Zenject;

namespace Scenes.SessionRework.Scripts.World.Factories
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