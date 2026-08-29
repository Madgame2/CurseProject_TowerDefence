using System.Collections.Generic;
using System.Linq;
using Scenes.SessionRework.Scripts.GameWorld.Core.interfaces.BackendParams;
using Scenes.SessionRework.Scripts.GameWorld.Graph.BiomGraph.Interfaces;
using Scenes.SessionRework.Scripts.GameWorld.Graph.DecorationsGrpah.Interfaces;
using Scenes.SessionRework.Scripts.GameWorld.Graph.LandscapeGraph.Interfaces;
using Scenes.SessionRework.Scripts.GameWorld.Interfaces;
using Scenes.SessionRework.Scripts.Services.Sync.DTO.PlayersInitDTOs;

namespace Scenes.SessionRework.Scripts.GameWorld.Core.Meta
{
    public class BackendParamStorage: IBackendParamStorage
    {
        private List<PlayaerMetaDataDTO> _playersArray = new List<PlayaerMetaDataDTO>();
        public IChunksSettings ChunksSettings { get; set; }
        public ILandscapeGraphNode LandscapeGraphRoot { get; set; }
        public IBiomeGraphNode BiomeGraphRoot { get; set; }
        public IDecorationsGraphNode DecorationsGraphRoot { get; set; }
        public uint UdpToken { get; set; }

        ICollection<PlayaerMetaDataDTO> ISetBackendParam.PlayersArray
        {
            get => _playersArray;
            set => _playersArray = value as List<PlayaerMetaDataDTO> ?? value?.ToList() ?? new List<PlayaerMetaDataDTO>();
        }

        IReadOnlyCollection<PlayaerMetaDataDTO> IGetBackendParam.PlayersArray => _playersArray;
    }
}