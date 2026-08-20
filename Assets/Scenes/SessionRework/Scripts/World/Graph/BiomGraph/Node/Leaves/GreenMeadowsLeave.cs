using Scenes.SessionRework.Scripts.GameWorld.Graph.BiomGraph.Meta.Enums;
using Scenes.SessionRework.Scripts.GameWorld.Graph.BiomGraph.Node.Base;
using Scenes.SessionRework.Scripts.GameWorld.Graph.Nodes.Attributes;
using Scenes.SessionRework.Scripts.Services.Sync.DTO.WorldGenerationGraph.Meta;

namespace Scenes.SessionRework.Scripts.GameWorld.Graph.BiomGraph.Node.Leaves
{
    [NodeType(NodeType.GreenMeadowsNode)]
    public class GreenMeadowsLeave: LeaveBase
    {
        public override BiomeType Evaluate(float x, float y)
        {
            if(x%2 == 0 && y%2 == 0)
                return BiomeType.STONE_AREA;
            
            return  BiomeType.GREEN_MEADOWS;
        }

        public override void Initialize(NodeParam[] parameters)
        { }
    }
}