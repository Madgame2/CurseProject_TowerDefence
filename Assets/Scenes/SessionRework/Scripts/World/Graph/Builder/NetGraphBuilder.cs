using Scenes.SessionRework.Scripts.GameWorld.Graph.Interfaces;
using Scenes.SessionRework.Scripts.GameWorld.Interfaces;
using Scenes.SessionRework.Scripts.Services.Sync.DTO.WorldGenerationGraph;
using Zenject;

namespace Scenes.SessionRework.Scripts.GameWorld.Graph.Builder
{
    public class NetGraphBuilder: INetGraphsBuilder
    {
        [Inject] private readonly INodeFactory _nodeFactory;
        
        public IGraphNode CreateGrpah(GraphNodeDTO[] GraphDTOs)
        {
            if (GraphDTOs == null || GraphDTOs.Length == 0)
                return null;
            
            IGraphNode[] instantiatedNodes = new IGraphNode[GraphDTOs.Length];
            
            for (int i = 0; i < GraphDTOs.Length; i++)
            {
                var dto = GraphDTOs[i];
                
                IGraphNode node = _nodeFactory.CreateNode(dto.Type);
                
                node.Initialize(dto.Params);
            
                instantiatedNodes[i] = node;
            }
            
            IGraphNode rootNode = null;
            
            for (int i = 0; i < GraphDTOs.Length; i++)
            {
                var dto = GraphDTOs[i];
                var currentNode = instantiatedNodes[i];
                
                if (dto.ParentNode == -1)
                {
                    rootNode = currentNode;
                }
                
                if (dto.ChildNodes != null)
                {
                    foreach (short childIndex in dto.ChildNodes)
                    {
                        currentNode.AddChild(instantiatedNodes[childIndex]);
                    }
                }
            }
            
            return rootNode;
        }
    }
}