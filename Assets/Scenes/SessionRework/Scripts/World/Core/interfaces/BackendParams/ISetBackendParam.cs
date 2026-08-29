using System.Collections.Generic;
using Scenes.SessionRework.Scripts.GameWorld.Graph.BiomGraph.Interfaces;
using Scenes.SessionRework.Scripts.GameWorld.Graph.DecorationsGrpah.Interfaces;
using Scenes.SessionRework.Scripts.GameWorld.Graph.LandscapeGraph.Interfaces;
using Scenes.SessionRework.Scripts.GameWorld.Interfaces;
using Scenes.SessionRework.Scripts.Services.Sync.DTO.PlayersInitDTOs;

namespace Scenes.SessionRework.Scripts.GameWorld.Core.interfaces.BackendParams
{
    public interface ISetBackendParam
    {
        IChunksSettings  ChunksSettings { set; }
        ILandscapeGraphNode LandscapeGraphRoot { set; }
        IBiomeGraphNode BiomeGraphRoot { set; }
        IDecorationsGraphNode DecorationsGraphRoot { set; }
        ICollection<PlayaerMetaDataDTO> PlayersArray { get; set; }
        uint UdpToken { set; }
    }
}