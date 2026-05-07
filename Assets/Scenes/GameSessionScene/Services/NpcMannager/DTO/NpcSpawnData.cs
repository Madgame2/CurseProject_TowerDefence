using System;
using UnityEngine;

[Serializable]
public class NpcSpawnData : INpcEventData
{
    public Vector2Dto position;
    public Npcbehavior behaver;
}
