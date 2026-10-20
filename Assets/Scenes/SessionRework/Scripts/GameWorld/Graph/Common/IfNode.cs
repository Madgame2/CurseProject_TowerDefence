using System;
using System.Linq;
using GameServer.Domain.SessionWorld.Graphs.Base;
using JetBrains.Annotations;
using Scenes.SessionRework.Scripts.GameWorld.Graph.Common.Conditions;
using Scenes.SessionRework.Scripts.GameWorld.Graph.Common.Conditions.Interfaces;
using Scenes.SessionRework.Scripts.GameWorld.Graph.Interfaces;
using Scenes.SessionRework.Scripts.GameWorld.Graph.Nodes.Attributes;
using Scenes.SessionRework.Scripts.Services.Sync.DTO.WorldGenerationGraph.Meta;
using Unity.VisualScripting;

namespace Scenes.SessionRework.Scripts.GameWorld.Graph.Common
{
    [NodeType(NodeType.IfNode)]
    public class IfNode<T> : IGraphNode<T>
    {
        private ICondition _condition;
        
        public IGraphNode<T> ThenNode { get; private set; }
        [CanBeNull] public IGraphNode<T> ElseNode { get; private set; }
        
        public bool _thenNodeInited = false;
        
        public IfNode() {}
        
        public bool TryEvaluate(float x, float y, out T  output)
        {
            output = default;
            
            if (_condition.Pass(x, y))
            {
                return ThenNode != null && ThenNode.TryEvaluate(x, y, out output);
            }

            if (ElseNode != null)
            {
                return ElseNode.TryEvaluate(x, y, out var output1);
            }

            return false;
        }

        public IGraphNode[] GetChildren()
        {
            return new []{ThenNode, ElseNode};
        }

        public void Initialize(NodeParam[] parameters, IGraphNode[] allNodes)
        {
            var condParam = parameters.FirstOrDefault(p => p.Param == ParamsType.Condition);

            if (condParam.ValueType == ParamValueType.Condition)
            {
                var meta = condParam.ConditionValue.Value;
                
                switch (meta.CompareDataType)
                {
                    case ParamValueType.Float:
                        var nodeAFloat = (IGraphNode<float>)allNodes[meta.InputAId];
                        var nodeBFloat = (IGraphNode<float>)allNodes[meta.InputBId];
                        _condition = new CompareCondition<float>(nodeAFloat, nodeBFloat, meta.CompareMode);
                        break;

                    case ParamValueType.Int:
                        var nodeAInt = (IGraphNode<int>)allNodes[meta.InputAId];
                        var nodeBInt = (IGraphNode<int>)allNodes[meta.InputBId];
                        _condition = new CompareCondition<int>(nodeAInt, nodeBInt, meta.CompareMode);
                        break;

                    case ParamValueType.Bool:
                        var nodeABool = (IGraphNode<bool>)allNodes[meta.InputAId];
                        var nodeBBool = (IGraphNode<bool>)allNodes[meta.InputBId];
                        _condition = new CompareCondition<bool>(nodeABool, nodeBBool, meta.CompareMode);
                        break;

                    default:
                        throw new Exception($"Неподдерживаемый тип сравнения: {meta.CompareDataType}");
                }
            }
        }

        public void AddChild(IGraphNode child)
        {
            if (!_thenNodeInited)
            {
                ThenNode = (IGraphNode<T>)child;
                _thenNodeInited = true;
                return;
            }
            ElseNode = (IGraphNode<T>)child;
        }
    }
}