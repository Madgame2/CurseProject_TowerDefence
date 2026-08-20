using Scenes.SessionRework.Scripts.GameWorld.Graph.DecorationsGrpah.DecorationRulesModels;
using Scenes.SessionRework.Scripts.GameWorld.Graph.DecorationsGrpah.Interfaces;
using Scenes.SessionRework.Scripts.GameWorld.Graph.BiomGraph.Interfaces;

namespace Scenes.SessionRework.Scripts.GameWorld.Interfaces
{
    public interface IDecorationGraphBuilder
    {
        IDecorationsGraphNode CreateGraph(DecorationRules rulesObject);
    }
}