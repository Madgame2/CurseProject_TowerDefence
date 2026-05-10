using Newtonsoft.Json.Linq;
using UnityEngine;

public enum GrossCannonUpdateTypes
{
    NONE,
    ACTION
}

public enum GrossCannonActionTypes
{
    NONE,
    SHOOT,
    SET_TARGET 
}

public class GrossCannonUpdatesDTO
{
    public GrossCannonUpdateTypes updateType;
    public JObject data;
}

public class ActionDTO
{
    public GrossCannonActionTypes type;
    public JObject data;
}

public class setTargetDTO
{
    public string? target;
}

public class ShootToTargetDTO
{
    public string? target;
}