using System;
using UnityEngine;

[Serializable]
public class NpcUpdateData : INpcEventData
{
    public Vector2Dto position;
    public Vector3Dto rotation;
    public Vector2Dto velocity;
}