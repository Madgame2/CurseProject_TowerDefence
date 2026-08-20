using System;
using Scenes.SessionRework.Scripts.GameWorld.Graph.BiomGraph.Interfaces;
using Scenes.SessionRework.Scripts.GameWorld.Graph.BiomGraph.Meta.Enums;
using Scenes.SessionRework.Scripts.GameWorld.Graph.Interfaces;
using Scenes.SessionRework.Scripts.Services.Sync.DTO.WorldGenerationGraph.Meta;

namespace Scenes.SessionRework.Scripts.GameWorld.Graph.BiomGraph.Node.Base
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