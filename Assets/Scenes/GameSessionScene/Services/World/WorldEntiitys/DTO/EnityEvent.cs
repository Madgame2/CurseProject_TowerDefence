using Newtonsoft.Json;
using UnityEngine;


public enum EntityEventType
{
    SPAWN,
    TERMINATE,
    UPDATE
}

public enum EntityesEnum
{
    RootHouse,

    GrossCannonInBuild,
    GrossCannon,

    TeslaTowerBuild,
    TeslaTower,

    CampInBuild,
    Camp
}

public interface IWorldUpdateState
{
}


public class EntityEvent : IWorldUpdateState
{
    [JsonProperty("enityId")]
    public string EnityId { get; set; }

    [JsonProperty("enventType")]
    public EntityEventType EnventType { get; set; }

    [JsonProperty("enityType")]
    public EntityesEnum EnityType { get; set; }

    [JsonProperty("data")]
    public object Data { get; set; }
}
