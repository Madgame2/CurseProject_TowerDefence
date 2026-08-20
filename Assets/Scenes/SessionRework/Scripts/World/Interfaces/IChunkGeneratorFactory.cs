using Scenes.SessionRework.Scripts.GameWorld.Graph.BiomGraph.Interfaces;
using Scenes.SessionRework.Scripts.GameWorld.Graph.DecorationsGrpah.Interfaces;
using Scenes.SessionRework.Scripts.GameWorld.Graph.LandscapeGraph.Interfaces;
using Scenes.SessionRework.Scripts.GameWorld.Graph.Interfaces;

namespace Scenes.SessionRework.Scripts.GameWorld.Interfaces
{
    public interface IChunkGeneratorFactory
    {
        IChunkGenerator Create(ILandscapeGraphNode landscapeGraph, IBiomeGraphNode biomeGraph,IDecorationsGraphNode decorationsGraph);
    }
}