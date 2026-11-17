using System;
using System.Xml.Serialization;

namespace Scenes.SessionRework.Scripts.Network.Meta.EntityComponents.Base.DTO
{
    [Serializable]
    [XmlRoot("Metadata")]
    public sealed class NetComponentMetadataDto
    {
        public int Version { get; set; }
    }
}