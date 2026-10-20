using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scenes.SessionRework.Scripts.GameWorld.Graph.Interfaces;
using Scenes.SessionRework.Scripts.GameWorld.Graph.Nodes.Attributes;
using Scenes.SessionRework.Scripts.GameWorld.Graph.TreesGraph.Interfaces;
using Scenes.SessionRework.Scripts.GameWorld.Graph.TreesGraph.Models;
using Scenes.SessionRework.Scripts.Services.Sync.DTO.WorldGenerationGraph.Meta;
using UnityEngine;

namespace GameServer.Domain.SessionWorld.Graphs.TreeGraph.Leaves
{
    [NodeType(NodeType.CreateTreeNode)]
    public class CreateTreeLeave : ITreeGraphNode
    {
        public NodeType Type => NodeType.CreateTreeNode;

        public bool TryEvaluate(float x, float y, out StaticTreeData  output)
        {
            output = new StaticTreeData { LocalPosition = new Vector2(x, y) };
            return true;
        }

        public IGraphNode[] GetChildren()
        {
            return Array.Empty<IGraphNode>();
        }

        public void Initialize(NodeParam[] parameters, IGraphNode[] allNodes)
        {
            
        }

        public void AddChild(IGraphNode child)
        {
            
        }

        public NodeParam[] GetParams(Func<IGraphNode, short> serializeCallback)
        {
            return Array.Empty<NodeParam>();
        }
    }
}
