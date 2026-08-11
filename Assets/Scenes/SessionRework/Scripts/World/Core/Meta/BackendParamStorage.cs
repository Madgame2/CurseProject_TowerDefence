using Scenes.SessionRework.Scripts.World.Core.interfaces.BackendParams;
using Scenes.SessionRework.Scripts.World.Graph.BiomGraph.Interfaces;
using Scenes.SessionRework.Scripts.World.Graph.LandscapeGraph.Interfaces;
using Scenes.SessionRework.Scripts.World.Interfaces;

namespace Scenes.SessionRework.Scripts.World.Core.Meta
{
    public class BackendParamStorage: IBackendParamStorage
    {
        public IChunksSettings ChunksSettings { get; set; }
        public ILandscapeGraphNode LandscapeGraphRoot { get; set; }
        public IBiomeGraphNode BiomeGraphRoot { get; set; }
    }
}