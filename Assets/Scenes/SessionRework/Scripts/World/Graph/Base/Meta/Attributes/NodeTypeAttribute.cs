using System;
using Scenes.SessionRework.Scripts.Services.Sync.DTO.WorldGenerationGraph.Meta;

namespace Scenes.SessionRework.Scripts.World.Graph.Nodes.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class NodeTypeAttribute: Attribute
    {
        public NodeType Type { get; }

        public NodeTypeAttribute(NodeType type)
        {
            Type = type;
        }   
    }
}