using System;
using Scenes.SessionRework.Scripts.GameWorld.Graph.Base;
using Scenes.SessionRework.Scripts.GameWorld.Graph.Interfaces;
using Scenes.SessionRework.Scripts.GameWorld.Graph.Nodes.Attributes;
using Scenes.SessionRework.Scripts.Services.Sync.DTO.WorldGenerationGraph.Meta;

namespace Scenes.SessionRework.Scripts.GameWorld.Graph.Nodes.Leaves
{
    [NodeType(NodeType.PerlinNoiseNode)]
    public class PerlinNoiseLeaf : GraphLeaveBase
    {
        private readonly DotnetNoise.FastNoise _noise = new();

        public float Frequency
        {
            get => _noise.Frequency;
            set => _noise.Frequency = value;
        }

        public PerlinNoiseLeaf()
        {
            _noise.Frequency = 0.05f;
        }

        public override float Evaluate(float x, float y)
        {
            float rawNoise = _noise.GetPerlin(x, y);

            return (rawNoise + 1.0f) / 2.0f;
        }

        public override IGraphNode[] GetChildren()
        {
            return Array.Empty<IGraphNode>();
        }

        public override void Initialize(NodeParam[] parameters)
        {
            if (parameters == null) return;

            foreach (var param in parameters)
            {
                if (param.Param == ParamsType.Frequency && param.ValueType == ParamValueType.Float)
                {
                    Frequency = param.FloatValue;
                }
            }
        }
    }
}