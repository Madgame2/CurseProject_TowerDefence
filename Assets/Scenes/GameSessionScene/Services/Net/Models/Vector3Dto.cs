using Newtonsoft.Json;
using UnityEngine;

public class Vector3Dto
{
    public Vector3Dto()
    {

    }


    public Vector3Dto(float  x, float y, float z)
    {
        this.X = x; this.Y = y; this.Z = z;
    }

    [JsonProperty("x")]
    public float X { get; set; }

    [JsonProperty("y")]
    public float Y { get; set; }

    [JsonProperty("z")]
    public float Z { get; set; }
}


public class Vector2Dto
{
    [JsonProperty("x")]
    public float X { get; set; }

    [JsonProperty("y")]
    public float Y { get; set; }
}