using Lobby.NavSystem;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PathConfig", menuName = "Scriptable Objects/PathConfig")]
public class WaysDataBase : ScriptableObject
{
    public List<Path> ways;

    public Path GetPath(string name) => ways.Find(w => w.PathName == name);
}
