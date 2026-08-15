using Scenes.SessionRework.Scripts.World.Graph.BiomGraph.Interfaces;
using Scenes.SessionRework.Scripts.World.Graph.DecorationsGrpah.DecorationRulesModels;
using Scenes.SessionRework.Scripts.World.Graph.DecorationsGrpah.Interfaces;

namespace Scenes.SessionRework.Scripts.World.Interfaces
{
    public interface IDecorationGraphBuilder
    {
        IDecorationsGraphNode CreateGraph(DecorationRules rulesObject);
    }
}