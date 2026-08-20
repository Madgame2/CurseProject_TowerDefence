using System;
using Scenes.SessionRework.Scripts.Services.Sync.DTO.WorldGenerationGraph.Meta;

namespace Scenes.SessionRework.Scripts.GameWorld.Graph.Nodes.Factory.Exceptions
{
    public class CanNotCreateGraphNodeException : Exception
    {
        public CanNotCreateGraphNodeException(NodeType type)
            :base($"CanNotCreateGraphNodeException: cant create graph node, type: {type}")
        { }
    }
}