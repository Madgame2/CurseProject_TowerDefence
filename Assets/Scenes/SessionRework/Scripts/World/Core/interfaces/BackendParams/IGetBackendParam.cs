using Scenes.SessionRework.Scripts.World.Graph.BiomGraph.Interfaces;
using Scenes.SessionRework.Scripts.World.Graph.LandscapeGraph.Interfaces;
using Scenes.SessionRework.Scripts.World.Interfaces;

namespace Scenes.SessionRework.Scripts.World.Core.interfaces.BackendParams
{
    public interface IGetBackendParam
    {
        IChunksSettings  ChunksSettings { get; }
        ILandscapeGraphNode LandscapeGraphRoot { get; }
        IBiomeGraphNode BiomeGraphRoot { get; }
    }
}