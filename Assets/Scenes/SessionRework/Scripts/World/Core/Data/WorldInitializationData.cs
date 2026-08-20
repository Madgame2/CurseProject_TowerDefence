using Scellecs.Morpeh;
using Scenes.SessionRework.Scripts.GameWorld.Graph.BiomGraph.Interfaces;
using Scenes.SessionRework.Scripts.GameWorld.Graph.DecorationsGrpah.Interfaces;
using Scenes.SessionRework.Scripts.GameWorld.Graph.LandscapeGraph.Interfaces;
using Scenes.SessionRework.Scripts.GameWorld.Interfaces;

namespace Scenes.SessionRework.Scripts.GameWorld.Core.Data
{
    public class WorldInitializationData
    {
        public IChunksSettings Settings { get; }
        public ILandscapeGraphNode LandscapeGraph { get; }
        public IBiomeGraphNode BiomeGraph { get; }
        public IDecorationsGraphNode DecorationsGraph { get; }
        public World EcsWorld { get; }

        public WorldInitializationData(
            IChunksSettings settings, 
            ILandscapeGraphNode landscapeGraph, 
            IBiomeGraphNode biomeGraph,
            IDecorationsGraphNode decorationsGraph,
            World world)
        {
            Settings = settings;
            LandscapeGraph = landscapeGraph;
            BiomeGraph = biomeGraph;
            DecorationsGraph = decorationsGraph;
            EcsWorld = world;
        }
    }
}