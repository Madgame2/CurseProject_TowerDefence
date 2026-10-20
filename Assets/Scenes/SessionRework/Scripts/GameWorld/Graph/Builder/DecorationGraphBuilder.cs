using Scenes.SessionRework.Scripts.GameWorld.Graph.DecorationsGrpah.DecorationRulesModels;
using Scenes.SessionRework.Scripts.GameWorld.Graph.DecorationsGrpah.Interfaces;
using Scenes.SessionRework.Scripts.GameWorld.Graph.DecorationsGrpah.Node.Leaves;
using Scenes.SessionRework.Scripts.GameWorld.Interfaces;

namespace Scenes.SessionRework.Scripts.GameWorld.Graph.Builder
{
    public class DecorationGraphBuilder: IDecorationGraphBuilder
    {
        public IDecorationsGraphNode CreateGraph(DecorationRules rulesObject)
        {
            return new GrassLeaveNode();
        }
    }
}