using System.Linq;
using GameServer.Domain.SessionWorld.Graphs.Base;
using GameServer.Domain.SessionWorld.Graphs.Base.Meta;
using Scenes.SessionRework.Scripts.GameWorld.Graph.Interfaces;
using Scenes.SessionRework.Scripts.GameWorld.Interfaces;
using Scenes.SessionRework.Scripts.Services.Sync.DTO.WorldGenerationGraph;
using Zenject;

namespace Scenes.SessionRework.Scripts.GameWorld.Graph.Builder
{
    public class NetGraphBuilder: INetGraphsBuilder
    {
        [Inject] private readonly INodeFactory _nodeFactory;
        
        public IGraphNode CreateGrpah(GraphNodeDTO[] graphDTOs)
        {
            if (graphDTOs == null || graphDTOs.Length == 0)
                return null;
            
            IGraphNode[] instantiatedNodes = new IGraphNode[graphDTOs.Length];
            
            for (int i = 0; i < graphDTOs.Length; i++)
            {
                var genericParam = graphDTOs[i].Params?.FirstOrDefault(p => p.GenericType != GenericTypes.None);
                
                if (genericParam.HasValue && genericParam.Value.GenericType != GenericTypes.None)
                {
                    var genericType = DataTypeRegistry.GetType(genericParam.Value.GenericType);
                    instantiatedNodes[i] = _nodeFactory.CreateNode(graphDTOs[i].Type, genericType);
                }
                else
                {
                    instantiatedNodes[i] = _nodeFactory.CreateNode(graphDTOs[i].Type);
                }
            }
            
            for (int i = 0; i < graphDTOs.Length; i++)
            {
                var dto = graphDTOs[i];
                var currentNode = instantiatedNodes[i];
            
                currentNode.Initialize(dto.Params, instantiatedNodes);
            
                if (dto.ChildNodes != null)
                {
                    foreach (short childIndex in dto.ChildNodes)
                    {
                        if (childIndex != -1)
                        {
                            currentNode.AddChild(instantiatedNodes[childIndex]);
                        }
                    }
                }
            }
            
            return instantiatedNodes[0];
        }
    }
}