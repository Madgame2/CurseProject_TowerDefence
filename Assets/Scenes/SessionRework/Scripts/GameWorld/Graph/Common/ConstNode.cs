using System;
using System.Linq;
using Scenes.SessionRework.Scripts.GameWorld.Graph.Interfaces;
using Scenes.SessionRework.Scripts.GameWorld.Graph.Nodes.Attributes;
using Scenes.SessionRework.Scripts.Services.Sync.DTO.WorldGenerationGraph.Meta;

namespace Scenes.SessionRework.Scripts.GameWorld.Graph.Common
{
    [NodeType(NodeType.ConstNode)]
    public class ConstNode<T> : IGraphNode<T>
    {
        public T _const;

        public NodeType Type => NodeType.ConstNode;

        public ConstNode(){}
        
        public ConstNode(T value)
        {
            _const = value;
        }

        public bool TryEvaluate(float x, float y, out T  output)
        {
            output =  _const;
            return true;
        }

        public IGraphNode[] GetChildren()
        {
            return Array.Empty<IGraphNode>();
        }

        public void Initialize(NodeParam[] parameters, IGraphNode[] allNodes)
        {
            var param = parameters.FirstOrDefault(p => p.Param == ParamsType.ConstValue);

            // Определяем, какого типа T мы ожидаем, и достаем нужное поле
            if (typeof(T) == typeof(float))
            {
                _const = (T)(object)param.FloatValue;
            }
            else if (typeof(T) == typeof(int))
            {
                _const = (T)(object)param.IntValue;
            }
            else if (typeof(T) == typeof(bool))
            {
                _const = (T)(object)param.BoolValue;
            }
            else if (typeof(T) == typeof(string))
            {
                _const = (T)(object)param.StringValue;
            }
            else
            {
                object rawValue = param.ValueType switch
                {
                    ParamValueType.Float => param.FloatValue,
                    ParamValueType.Int => param.IntValue,
                    ParamValueType.Bool => param.BoolValue,
                    ParamValueType.String => param.StringValue,
                    _ => default(T)
                };
                _const = (T)Convert.ChangeType(rawValue, typeof(T));
            }
        }

        public void AddChild(IGraphNode child)
        {
            return;
        }

        public NodeParam[] GetParams(Func<IGraphNode, short> serializeCallback)
        {
            var param = new NodeParam
            {
                Param = ParamsType.ConstValue
            };

            switch (_const)
            {
                case float f:
                    param.ValueType = ParamValueType.Float;
                    param.FloatValue = f;
                    break;

                case int i:
                    param.ValueType = ParamValueType.Int;
                    param.IntValue = i;
                    break;

                case double d:
                    param.ValueType = ParamValueType.Float;
                    param.FloatValue = (float)d;
                    break;

                case long l:
                    param.ValueType = ParamValueType.Int;
                    param.IntValue = (int)l;
                    break;

                case byte b:
                    param.ValueType = ParamValueType.Int;
                    param.IntValue = b;
                    break;

                case short s:
                    param.ValueType = ParamValueType.Int;
                    param.IntValue = s;
                    break;

                case bool bVal:
                    param.ValueType = ParamValueType.Bool;
                    param.BoolValue = bVal;
                    break;

                case string sVal:
                    param.ValueType = ParamValueType.String;
                    param.StringValue = sVal;
                    break;

                default:
                    param.ValueType = ParamValueType.Float;
                    param.FloatValue = Convert.ToSingle(_const);
                    break;
            }

            return new[] { param };
        }
    }
}