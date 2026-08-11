using Scenes.SessionRework.Scripts.Services.Sync.DTO.WorldGenerationGraph.Meta;

namespace Scenes.SessionRework.Scripts.Services.Sync.DTO.WorldGenerationGraph
{
    public class GraphNodeDTO
    {

        public NodeType Type { get; set;  }
        
        public short ParentNode { get; set; } = -1;
        
        public short[] ChildNodes { get; set; }
        
        public NodeParam[] Params{ get; set; }
    }
}