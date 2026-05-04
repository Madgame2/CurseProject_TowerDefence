using Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEngine;

public class ChankUpdate
{
    [JsonProperty("chankPos")]
    public Vector2Dto chankPos { get; set; }

    [JsonProperty("chankCell")]

    public Vector2Dto chankCell { get; set; }

    [JsonProperty("cellData")]
    public int cellData { get; set; }
}