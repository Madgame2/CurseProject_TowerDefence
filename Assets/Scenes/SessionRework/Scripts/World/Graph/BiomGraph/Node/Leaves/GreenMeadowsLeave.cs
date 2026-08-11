using Scenes.SessionRework.Scripts.Services.Sync.DTO.WorldGenerationGraph.Meta;
using Scenes.SessionRework.Scripts.World.Graph.BiomGraph.Meta.Enums;
using Scenes.SessionRework.Scripts.World.Graph.BiomGraph.Node.Base;
using Scenes.SessionRework.Scripts.World.Graph.Nodes.Attributes;

namespace Scenes.SessionRework.Scripts.World.Graph.BiomGraph.Node.Leaves
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