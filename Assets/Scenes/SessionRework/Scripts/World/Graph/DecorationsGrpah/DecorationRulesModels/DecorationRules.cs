using System.Collections.Generic;
using System.Xml.Serialization;

namespace Scenes.SessionRework.Scripts.GameWorld.Graph.DecorationsGrpah.DecorationRulesModels
{
    [XmlRoot("DecorationRules")]
    public class DecorationRules
    {
        [XmlAttribute("version")]
        public string Version { get; set; } = string.Empty;

        [XmlElement("Global")]
        public GlobalSettings Global { get; set; } = new();

        [XmlArray("Biomes")]
        [XmlArrayItem("Biome")]
        public List<Biome> Biomes { get; set; } = new();
    }
}