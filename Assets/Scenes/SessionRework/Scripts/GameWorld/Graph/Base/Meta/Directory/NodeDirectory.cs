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

        var nodeTypes = assembly.GetTypes().Where(t => 
            t.IsClass 
            && !t.IsAbstract 
            && (typeof(IGraphNode).IsAssignableFrom(t) || ImplementsOpenGenericInterface(t, typeof(IGraphNode<>))));
        
        
        foreach (var type in nodeTypes)
        {
            var attribute = type.GetCustomAttribute<NodeTypeAttribute>();
            
            if (attribute != null)
            {
                Type typeToRegister = type.IsGenericType ? type.GetGenericTypeDefinition() : type;
                _nodesPool.Add(attribute.Type, typeToRegister);
            }
        }
    }

    public bool TryGetValue(NodeType type, out Type nodeType)
    {
        return _nodesPool.TryGetValue(type, out nodeType);
    }
    
    public bool TryGetGenericValue(NodeType type, Type[] genericArguments, out Type nodeType)
    {
        if (_nodesPool.TryGetValue(type, out var openGenericType) && openGenericType.IsGenericType)
        {
            nodeType = openGenericType.MakeGenericType(genericArguments);
            return true;
        }

        nodeType = null;
        return false;
    }
    
    private bool ImplementsOpenGenericInterface(Type type, Type openGenericInterface)
    {
        return type.GetInterfaces().Any(i => 
            i.IsGenericType && 
            i.GetGenericTypeDefinition() == openGenericInterface);
    }
}
