using GameServer.Domain.SessionWorld.Graphs.Base.Meta;
using Scenes.SessionRework.Scripts.GameWorld.Graph.Common.Meta;

namespace Scenes.SessionRework.Scripts.Services.Sync.DTO.WorldGenerationGraph.Meta
{
    public struct NodeParam
    {
        public ParamsType Param { get; set; }

        public int IntValue;
        public float FloatValue;
        public bool BoolValue;
        public string StringValue;

        public ParamValueType ValueType;
        
        public ConditionData? ConditionValue;
        public GenericTypes GenericType;
    }
}