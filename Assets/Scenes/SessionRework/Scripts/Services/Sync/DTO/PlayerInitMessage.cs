using System.Collections.Generic;
using Scenes.SessionRework.Scripts.Services.Sync.DTO.PlayersInitDTOs;

namespace Scenes.SessionRework.Scripts.Services.Sync.DTO
{
    public class PlayerInitMessage
    {
        public List<PlayaerMetaDataDTO> Players { get; set; }=new List<PlayaerMetaDataDTO>();
    }
}