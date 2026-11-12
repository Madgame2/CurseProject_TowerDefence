using System;
using System.Numerics;

namespace Scenes.SessionRework.Scripts.Services.Sync.DTO.PlayersInitDTOs
{
    public class PlayaerMetaDataDTO
    {
        public uint PlayerId { get; set; }
        public Vector3 Position { get; set; }
        public  Quaternion Rotation { get; set; }
        public bool IsPlaying { get; set; }
        public uint ObjectId { get; set; }
    }
}