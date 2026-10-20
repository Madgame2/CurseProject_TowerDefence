using System;
using Scenes.SessionRework.Scripts.GameWorld.Graph.Base;
using Scenes.SessionRework.Scripts.GameWorld.Graph.Interfaces;
using Scenes.SessionRework.Scripts.GameWorld.Graph.Nodes.Attributes;
using Scenes.SessionRework.Scripts.Services.Sync.DTO.WorldGenerationGraph.Meta;
using UnityEngine;

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

        public override bool TryEvaluate(float x, float y, out float result)
        {
            float rawNoise = _noise.GetPerlin(x, y);
            result = (rawNoise + 1.0f) / 2.0f;

            return true;
        }

        public override IGraphNode[] GetChildren()
        {
            return Array.Empty<IGraphNode>();
        }

        public override void Initialize(NodeParam[] parameters, IGraphNode[] allNodes)
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