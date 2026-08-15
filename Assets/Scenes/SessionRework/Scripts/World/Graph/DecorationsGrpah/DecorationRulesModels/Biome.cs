using System.Collections.Generic;
using System.Xml.Serialization;
using Scenes.SessionRework.Scripts.World.Graph.BiomGraph.Meta.Enums;

namespace Scenes.SessionRework.Scripts.World.Graph.DecorationsGrpah.DecorationRulesModels
{
    public class Biome
    {
        [XmlAttribute("type")] public BiomeType Type { get; set; } = BiomeType.NONE;
        
        [XmlArray("Decorations")]
        [XmlArrayItem("Decoration")]
        public List<Decoration> Decorations { get; set; } = new();
    }
}