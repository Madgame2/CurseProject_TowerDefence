using System;
using UnityEngine;


public enum DataType
{
    NPC_STATE,
    ACTION
}

public enum ActionTypes
{
    ATTACK
}

[Serializable]
public class NpcUpdateData : INpcEventData
{
    public DataType dataType;
    public ActionTypes action;
    public Vector2Dto position;
    public Vector3Dto rotation;
    public Vector2Dto velocity;
}