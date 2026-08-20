using System.Collections.Generic;
using System.Data;
using System.Xml.Serialization;
using Scenes.SessionRework.Scripts.GameWorld.Graph.DecorationsGrpah.Meta.Enum;

namespace Scenes.SessionRework.Scripts.GameWorld.Graph.DecorationsGrpah.DecorationRulesModels
{
    public class Decoration
    {
        [XmlAttribute("type")]
        public DecorationType Type { get; set; } = DecorationType.NONE;

        [XmlAttribute("frequency")]
        public float Frequency { get; set; }

        [XmlArray("SpawnRules")]
        [XmlArrayItem("Rule")]
        public List<SpawnRules> SpawnRules { get; set; } = new();

        [XmlElement("Constraints")]
        public Constraints Constraints { get; set; } = new();
    }
}