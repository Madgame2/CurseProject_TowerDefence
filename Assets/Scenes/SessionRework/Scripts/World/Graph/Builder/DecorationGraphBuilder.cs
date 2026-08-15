using Scenes.SessionRework.Scripts.World.Graph.DecorationsGrpah.DecorationRulesModels;
using Scenes.SessionRework.Scripts.World.Graph.DecorationsGrpah.Interfaces;
using Scenes.SessionRework.Scripts.World.Graph.DecorationsGrpah.Node.Leaves;
using Scenes.SessionRework.Scripts.World.Interfaces;

namespace Scenes.SessionRework.Scripts.World.Graph.Builder
{
    public class DecorationGraphBuilder: IDecorationGraphBuilder
    {
        public IDecorationsGraphNode CreateGraph(DecorationRules rulesObject)
        {
            return new GrassLeaveNode();
        }
    }
}