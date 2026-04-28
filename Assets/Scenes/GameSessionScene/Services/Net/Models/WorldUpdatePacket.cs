using Newtonsoft.Json;
using UnityEngine;

public class WorldUpdatePacket
{
    [JsonProperty("action")]
    public string Action { get; set; } = "world_update";

    [JsonProperty("data")]
    public WorldUpdateData Data { get; set; }

    public WorldUpdatePacket(WorldUpdateData data)
    {
        Data = data;
    }
}
