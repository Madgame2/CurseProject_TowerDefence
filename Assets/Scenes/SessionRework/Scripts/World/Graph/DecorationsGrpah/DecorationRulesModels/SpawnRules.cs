using System.Xml.Serialization;
using Scenes.SessionRework.Scripts.World.Graph.DecorationsGrpah.Meta.Enum;

namespace Scenes.SessionRework.Scripts.World.Graph.DecorationsGrpah.DecorationRulesModels
{
    public class SpawnRules
    {
        [XmlAttribute("type")]
        public SpawnRuleType Type { get; set; } = SpawnRuleType.NONE;
    }
}