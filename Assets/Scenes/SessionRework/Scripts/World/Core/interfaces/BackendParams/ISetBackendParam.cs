using Scenes.SessionRework.Scripts.World.Graph.BiomGraph.Interfaces;
using Scenes.SessionRework.Scripts.World.Graph.LandscapeGraph.Interfaces;
using Scenes.SessionRework.Scripts.World.Interfaces;

namespace Scenes.SessionRework.Scripts.World.Core.interfaces.BackendParams
{
    public interface ISetBackendParam
    {
        IChunksSettings  ChunksSettings { set; }
        ILandscapeGraphNode LandscapeGraphRoot { set; }
        IBiomeGraphNode BiomeGraphRoot { set; }
    }
}