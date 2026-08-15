using Scenes.SessionRework.Scripts.World.Graph.BiomGraph.Interfaces;
using Scenes.SessionRework.Scripts.World.Graph.DecorationsGrpah.Interfaces;
using Scenes.SessionRework.Scripts.World.Graph.LandscapeGraph.Interfaces;
using Scenes.SessionRework.Scripts.World.Interfaces;

namespace Scenes.SessionRework.Scripts.World.Core.Data
{
    public class WorldInitializationData
    {
        public IChunksSettings Settings { get; }
        public ILandscapeGraphNode LandscapeGraph { get; }
        public IBiomeGraphNode BiomeGraph { get; }
        public IDecorationsGraphNode DecorationsGraph { get; }

        public WorldInitializationData(
            IChunksSettings settings, 
            ILandscapeGraphNode landscapeGraph, 
            IBiomeGraphNode biomeGraph,
            IDecorationsGraphNode decorationsGraph)
        {
            Settings = settings;
            LandscapeGraph = landscapeGraph;
            BiomeGraph = biomeGraph;
            DecorationsGraph = decorationsGraph;
        }
    }
}