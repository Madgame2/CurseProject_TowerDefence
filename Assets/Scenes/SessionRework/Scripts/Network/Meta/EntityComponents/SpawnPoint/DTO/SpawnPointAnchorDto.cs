using System;

namespace Scenes.SessionRework.Scripts.Network.Meta.EntityComponents.SpawnPoint.DTO
{
    [Serializable]
    public sealed class SpawnPointAnchorDto
    {
        public string AnchorName { get; set; } = string.Empty;
        
        public Vector3Dto SpawnPoint { get; set; }
        public Vector3Dto PivotPoint { get; set; }
    }

}