using System;
using System.Collections.Generic;
using Scenes.SessionRework.Scripts.GameWorld.Graph.Common.Conditions.Interfaces;
using Scenes.SessionRework.Scripts.GameWorld.Graph.Common.Meta;
using Scenes.SessionRework.Scripts.GameWorld.Graph.Common.Meta.Enums;
using Scenes.SessionRework.Scripts.GameWorld.Graph.Interfaces;
using Scenes.SessionRework.Scripts.Services.Sync.DTO.WorldGenerationGraph.Meta;

namespace Scenes.SessionRework.Scripts.GameWorld.Graph.Common.Conditions
{
    public class CompareCondition<T> : ICondition where T: IComparable<T>
    {
        private readonly IGraphNode<T> _a;
        private readonly IGraphNode<T> _b;
        private readonly CompareMode _mode;
        private readonly IComparer<T> _comparer;

        public CompareCondition(IGraphNode<T> a, IGraphNode<T> b, CompareMode mode)
        {
            _a = a;
            _b = b;
            _mode = mode;
            _comparer = Comparer<T>.Default;
        }

        public IGraphNode[] GetConditionNodes() => new IGraphNode[] { _a, _b };

        public ConditionData GetMetaData(Func<IGraphNode, short> serializeCallback)
        {
            ParamValueType compareType = ParamValueType.Float; 

            if (typeof(T) == typeof(int)) compareType = ParamValueType.Int;
            else if (typeof(T) == typeof(bool)) compareType = ParamValueType.Bool;
            else if (typeof(T) == typeof(string)) compareType = ParamValueType.String;

            return new ConditionData
            {
                Type = ConditionType.Compare,
                CompareMode = _mode,
                InputAId = serializeCallback(_a),
                InputBId = serializeCallback(_b),

                CompareDataType = compareType
            };
        }

        public bool Pass(float x, float y)
        {
            _a.TryEvaluate(x, y, out T valA);
            _b.TryEvaluate(x, y, out T valB );

            int result = _comparer.Compare(valA, valB);

            return _mode switch
            {
                CompareMode.Equal => result == 0,
                CompareMode.NotEqual => result != 0,
                CompareMode.Greater => result > 0,
                CompareMode.GreaterOrEqual => result >= 0,
                CompareMode.Less => result < 0,
                CompareMode.LessOrEqual => result <= 0,
                _ => false
            };
        }
    }
}