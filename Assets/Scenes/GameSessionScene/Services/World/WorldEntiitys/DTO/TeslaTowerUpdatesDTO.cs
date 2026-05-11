using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UnityEngine;

public enum TeslaTowerActionTypes{
    NONE,
    ATTACk
}

public class TeslaTowerUpdatesDTO
{
    public TeslaTowerActionTypes actionType;
    public JObject data;
}

public class TeslaTowerAttackDTO
{
    public NpcIdTree tree;
}


public class NpcIdTree
{
    public string id { get; set; }
    public List<NpcIdTree> children { get; set; } = new();
}
