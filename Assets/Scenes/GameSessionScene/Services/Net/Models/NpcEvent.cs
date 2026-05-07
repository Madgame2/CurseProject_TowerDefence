using UnityEngine;

public enum NpcTypes
{
    SKELETON,
    KNIGHT
}

public enum Npcbehavior
{
    ENEMY,
    NEITRALL,
    GUARDION
}

public enum NpcEventType
{
    SPAWN,
    TERMINATE,
    UPDATE
}

public class NpcEvent
{
    public string npcId;
    public NpcEventType enventType;
    public NpcTypes npcType;
    public Newtonsoft.Json.Linq.JObject data;
}
