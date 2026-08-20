using System;
using System.Numerics;

namespace Scenes.SessionRework.Scripts.Services.Sync.DTO.PlayersInitDTOs
{
    public class PlayaerMetaDataDTO
    {
        public Guid Id { get; set; }
        public Vector3 Position { get; set; }
        public bool IsPlaying { get; set; }
    }
}