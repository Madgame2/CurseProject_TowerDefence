using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "World/Decoration Database")]
public class DecorationConfigDatabase : ScriptableObject
{
    public DecorationConfig[] configs;

    private Dictionary<long, DecorationConfig> Llookup;
    private Dictionary<string, DecorationConfig> Slookup;

    public void Init()
    {
        Llookup = new Dictionary<long, DecorationConfig>();
        Slookup = new Dictionary<string, DecorationConfig>();


        foreach (var config in configs)
        {
            Llookup[config.id] = config;
            Slookup[config.name] = config;
        }
    }

    public DecorationConfig Get(string id)
    {
        return Slookup[id];
    }

    public DecorationConfig Get(long id)
    {
        return Llookup[id];
    }
}