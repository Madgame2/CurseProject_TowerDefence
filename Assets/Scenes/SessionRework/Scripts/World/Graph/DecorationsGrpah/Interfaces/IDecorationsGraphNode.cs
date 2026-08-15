using Scenes.SessionRework.Scripts.World.Graph.DecorationsGrpah.Meta.Enum;
using Scenes.SessionRework.Scripts.World.Graph.Interfaces;

namespace Scenes.SessionRework.Scripts.World.Graph.DecorationsGrpah.Interfaces
{
    public interface IDecorationsGraphNode: IGraphNode
    {
        public DecorationType  Evaluate(float x, float y);

    }
}