using Scenes.SessionRework.Scripts.GameWorld.Graph.DecorationsGrpah.DecorationRulesModels;

namespace Scenes.SessionRework.Scripts.Network.Parsers.Interfaces
{
    public interface IDecorationRulesParser
    {
        DecorationRules? Parse(byte[] data);
    }
}