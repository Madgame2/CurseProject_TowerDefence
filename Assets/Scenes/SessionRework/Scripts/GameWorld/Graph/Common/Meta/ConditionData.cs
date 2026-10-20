using Scenes.SessionRework.Scripts.Services.Sync.DTO.WorldGenerationGraph.Meta;
using Scenes.SessionRework.Scripts.GameWorld.Graph.Common.Meta.Enums;

namespace Scenes.SessionRework.Scripts.GameWorld.Graph.Common.Meta
{
    public struct ConditionData
    {
        public ConditionType Type { get; set; }
        public CompareMode CompareMode { get; set; }

        // Константные значения (для RangeCondition / ValueCompare)
        public float MinValue { get; set; }
        public float MaxValue { get; set; }
        public string TargetValue { get; set; }

        // Идентификаторы нод-входов (для CompareCondition между нодами)
        public int InputAId { get; set; }
        public int InputBId { get; set; }
        
        public ParamValueType CompareDataType;
    }
}