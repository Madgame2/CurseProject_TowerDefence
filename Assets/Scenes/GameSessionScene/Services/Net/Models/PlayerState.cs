using Newtonsoft.Json;
using UnityEngine;

public class PlayerState
{
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("position")]
    public Vector3Dto Position { get; set; }

    [JsonProperty("rotation")]
    public Vector3Dto Rotation { get; set; }

    [JsonProperty("velocity")]
    public Vector3Dto Velocity { get; set; }

    [JsonProperty("state")]
    public PlayerStates State { get; set; }
}


public enum PlayerStates
{
    IDEL,
    RUNING
}