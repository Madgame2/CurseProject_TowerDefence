using System;
using Scenes.SessionRework.Scripts.GameWorld.Graph.BiomGraph.Interfaces;
using Scenes.SessionRework.Scripts.GameWorld.Graph.DecorationsGrpah.Interfaces;
using Scenes.SessionRework.Scripts.GameWorld.Graph.DecorationsGrpah.Meta.Enum;
using Scenes.SessionRework.Scripts.GameWorld.Graph.Interfaces;
using Scenes.SessionRework.Scripts.Services.Sync.DTO.WorldGenerationGraph.Meta;
using Scenes.SessionRework.Scripts.GameWorld.Graph.BiomGraph.Meta.Enums;

namespace Scenes.SessionRework.Scripts.GameWorld.Graph.DecorationsGrpah.Node.Base
{
    public abstract class LeaveBase: IDecorationsGraphNode
    {
        public IGraphNode[] GetChildren()
        {
            return Array.Empty<IBiomeGraphNode>();
        }
        
        public void AddChild(IGraphNode child) { }
        
        public abstract DecorationType Evaluate(float x, float y);
        public abstract void Initialize(NodeParam[] parameters);

    }
}