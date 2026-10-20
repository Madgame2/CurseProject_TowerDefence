using System;
using Scenes.SessionRework.Scripts.GameWorld.Graph.Interfaces;
using Scenes.SessionRework.Scripts.GameWorld.Graph.Nodes.Factory.Exceptions;
using Scenes.SessionRework.Scripts.GameWorld.Interfaces;
using Scenes.SessionRework.Scripts.Services.Sync.DTO.WorldGenerationGraph.Meta;
using Zenject;

namespace Scenes.SessionRework.Scripts.GameWorld.Graph.Nodes.Factory
{
    public class NodeFactory : INodeFactory
    {
        [Inject] private readonly INodeDirectory _nodeDirectory;
        [Inject] private readonly DiContainer _container;

        public IGraphNode CreateNode(NodeType type, params Type[] genericArgs)
        {
            Type nodeType;
            
            if (genericArgs != null && genericArgs.Length > 0)
            {
                if (!_nodeDirectory.TryGetGenericValue(type, genericArgs, out nodeType))
                {
                    throw new CanNotCreateGraphNodeException(type);
                }
            }
            else
            {
                if (!_nodeDirectory.TryGetValue(type, out nodeType))
                {
                    throw new CanNotCreateGraphNodeException(type);
                }
            }

            var createdNode = (IGraphNode)_container.Instantiate(nodeType);            
            return createdNode;
        }
    }
}