using System.Collections.Generic;
using Scenes.SessionRework.Scripts.GameWorld.Graph.BiomGraph.Interfaces;
using Scenes.SessionRework.Scripts.GameWorld.Graph.DecorationsGrpah.Interfaces;
using Scenes.SessionRework.Scripts.GameWorld.Graph.LandscapeGraph.Interfaces;
using Scenes.SessionRework.Scripts.GameWorld.Interfaces;
using Scenes.SessionRework.Scripts.Services.Sync.DTO.PlayersInitDTOs;

namespace Scenes.SessionRework.Scripts.GameWorld.Core.interfaces.BackendParams
{
    public interface IGetBackendParam
    {
        IChunksSettings  ChunksSettings { get; }
        ILandscapeGraphNode LandscapeGraphRoot { get; }
        IBiomeGraphNode BiomeGraphRoot { get; }
        IDecorationsGraphNode DecorationsGraphRoot { get; }
        IReadOnlyCollection<PlayaerMetaDataDTO> PlayersArray { get; }
    }
}