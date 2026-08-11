using System;
using Scenes.SessionRework.Scripts.Services.Sync.DTO.WorldGenerationGraph.Meta;
using Scenes.SessionRework.Scripts.World.Graph.Interfaces;
using Scenes.SessionRework.Scripts.World.Graph.Nodes.Factory.Exceptions;
using Scenes.SessionRework.Scripts.World.Interfaces;
using Zenject;

namespace Scenes.SessionRework.Scripts.World.Graph.Nodes.Factory
{
    public class NodeFactory : INodeFactory
    {
        [Inject] private readonly INodeDirectory _nodeDirectory;
        [Inject] private readonly DiContainer _container;

        public IGraphNode CreateNode(NodeType type)
        {
            if (!_nodeDirectory.TryGetValue(type, out Type node))
            {
                throw new CanNotCreateGraphNodeException(type);
            }

            var createdNode = (IGraphNode)_container.Instantiate(node);
            
            return createdNode;
        }
    }
}