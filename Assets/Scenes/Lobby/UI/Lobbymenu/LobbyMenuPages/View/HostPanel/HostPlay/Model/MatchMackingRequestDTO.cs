using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class MatchMakingRequestDTO
{
    public long Seed;
    public string LobbyId;
    public MatchDifficulty matchDifficulty;
}


public enum MatchDifficulty
{
    Easy,
    Normal,
    Hard
}