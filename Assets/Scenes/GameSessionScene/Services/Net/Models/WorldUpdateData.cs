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

    [JsonProperty("chanks")]
    public List<ChankUpdate> chanks { get; set; } = new();

    [JsonProperty("enities")]
    public List<EntityEvent> enities { get; set; } = new();

    [JsonProperty("npc")]
    public List<NpcEvent> npc { get; set; } = new();

    [JsonProperty("director")]
    public List<DirectorEvent> director { get; set; } = new();
}
