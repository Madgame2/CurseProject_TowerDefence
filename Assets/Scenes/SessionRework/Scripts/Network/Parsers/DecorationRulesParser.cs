using System.IO;
using System.Xml.Serialization;
using Scenes.SessionRework.Scripts.Network.Parsers.Interfaces;
using Scenes.SessionRework.Scripts.GameWorld.Graph.DecorationsGrpah.DecorationRulesModels;

namespace Scenes.SessionRework.Scripts.Network.Parsers
{
    public class DecorationRulesParser : IDecorationRulesParser
    {
        public DecorationRules Parse(byte[] data)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(DecorationRules));
            using (var stream = new MemoryStream(data))
            {
                DecorationRules? rules = serializer.Deserialize(stream) as DecorationRules;
                    
                return rules;
            }
        }
    }
}