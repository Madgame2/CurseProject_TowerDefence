using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Scenes.SessionRework.Scripts.Services.Sync.DTO.WorldGenerationGraph.Meta
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum NodeType
    {
        NONE,
        PerlinNoiseNode,
        
        GreenMeadowsNode
    }
}