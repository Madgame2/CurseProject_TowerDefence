using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class WorldUpdateData
{
    [JsonProperty("tick")]
    public int Tick { get; set; }

    [JsonProperty("players")]
    public List<PlayerState> Players { get; set; } = new();
}
