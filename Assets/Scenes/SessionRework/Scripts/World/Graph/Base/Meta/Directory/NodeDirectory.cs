using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Scenes.SessionRework.Scripts.Services.Sync.DTO.WorldGenerationGraph.Meta;
using Scenes.SessionRework.Scripts.GameWorld.Graph.Interfaces;
using Scenes.SessionRework.Scripts.GameWorld.Graph.Nodes.Attributes;
using Scenes.SessionRework.Scripts.GameWorld.Interfaces;
using Zenject;


public class NodeDirectory : INodeDirectory, IInitializable
{
    private readonly Dictionary<NodeType, Type> _nodesPool = new Dictionary<NodeType, Type>();
    
    
    void IInitializable.Initialize()
    {
        Assembly assembly = Assembly.GetExecutingAssembly();

        var nodeTypes = assembly.GetTypes().Where(t=> t.IsClass 
                                                      && !t.IsAbstract
                                                      && typeof(IGraphNode).IsAssignableFrom(t));

        foreach (var type in nodeTypes)
        {
            var attribute = type.GetCustomAttribute<NodeTypeAttribute>();
            
            if (attribute != null)
            {
                _nodesPool.Add(attribute.Type, type);
            }
        }
    }

    public bool TryGetValue(NodeType type, out Type nodeType)
    {
        return _nodesPool.TryGetValue(type, out nodeType);
    }
}
