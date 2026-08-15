using Scenes.SessionRework.Scripts.World.Graph.BiomGraph.Interfaces;
using Scenes.SessionRework.Scripts.World.Graph.DecorationsGrpah.Interfaces;
using Scenes.SessionRework.Scripts.World.Graph.Interfaces;
using Scenes.SessionRework.Scripts.World.Graph.LandscapeGraph.Interfaces;

namespace Scenes.SessionRework.Scripts.World.Interfaces
{
    public interface IChunkGeneratorFactory
    {
        IChunkGenerator Create(ILandscapeGraphNode landscapeGraph, IBiomeGraphNode biomeGraph,IDecorationsGraphNode decorationsGraph);
    }
}