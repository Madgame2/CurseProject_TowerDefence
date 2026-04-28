using Newtonsoft.Json;
using UnityEngine;

public class WSMessage
{
    [JsonProperty("action")]
    public string? Action { get; set; }

    [JsonProperty("requestId")]
    public string? RequestId { get; set; }

    [JsonProperty("data")]
    public object? Data { get; set; }
}
