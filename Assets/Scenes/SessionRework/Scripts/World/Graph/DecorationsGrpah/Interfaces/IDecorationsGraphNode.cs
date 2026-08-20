using Scenes.SessionRework.Scripts.GameWorld.Graph.DecorationsGrpah.Meta.Enum;
using Scenes.SessionRework.Scripts.GameWorld.Graph.Interfaces;

namespace Scenes.SessionRework.Scripts.GameWorld.Graph.DecorationsGrpah.Interfaces
{
    public interface IDecorationsGraphNode: IGraphNode
    {
        public DecorationType  Evaluate(float x, float y);

    }
}