using Newtonsoft.Json.Linq;
using UnityEngine;

public enum MatchPhase
{
    PREPARATION,
    WAVE
}


public class DirectorEvent
{
    public MatchPhase matchPahase;
    public JObject data;
}
