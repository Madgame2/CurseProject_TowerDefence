using Scenes.SessionRework.Scripts.GameWorld.Graph.DecorationsGrpah.Meta.Enum;
using Scenes.SessionRework.Scripts.GameWorld.Graph.DecorationsGrpah.Node.Base;
using Scenes.SessionRework.Scripts.Services.Sync.DTO.WorldGenerationGraph.Meta;
using Scenes.SessionRework.Scripts.GameWorld.Graph.Base;
using Scenes.SessionRework.Scripts.GameWorld.Graph.Interfaces;

namespace Scenes.SessionRework.Scripts.GameWorld.Graph.DecorationsGrpah.Node.Leaves
{
    public class GrassLeaveNode: LeaveBase 
    {
        public override DecorationType Evaluate(float x, float y)
        {
            return DecorationType.GRASS;
        }

        public override void Initialize(NodeParam[] parameters)
        {
        }
    }
}