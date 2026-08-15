using System;
using Scenes.SessionRework.Scripts.Services.Sync.DTO.WorldGenerationGraph.Meta;
using Scenes.SessionRework.Scripts.World.Graph.BiomGraph.Interfaces;
using Scenes.SessionRework.Scripts.World.Graph.BiomGraph.Meta.Enums;
using Scenes.SessionRework.Scripts.World.Graph.Interfaces;

namespace Scenes.SessionRework.Scripts.World.Graph.BiomGraph.Node.Base
{
    public abstract class LeaveBase: IBiomeGraphNode
    {
        public IGraphNode[] GetChildren()
        {
            return Array.Empty<IBiomeGraphNode>();
        }
        
        public void AddChild(IGraphNode child) { }
        
        public abstract BiomeType Evaluate(float x, float y);
        public abstract void Initialize(NodeParam[] parameters);
    }
}