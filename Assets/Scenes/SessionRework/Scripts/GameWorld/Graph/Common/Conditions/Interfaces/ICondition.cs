using System;
using Scenes.SessionRework.Scripts.GameWorld.Graph.Common.Meta;
using Scenes.SessionRework.Scripts.GameWorld.Graph.Interfaces;

namespace Scenes.SessionRework.Scripts.GameWorld.Graph.Common.Conditions.Interfaces
{
    public interface ICondition
    {
        bool Pass(float x, float y);
        IGraphNode[] GetConditionNodes(); 
        ConditionData GetMetaData(Func<IGraphNode, short> serializeCallback);
    }
}