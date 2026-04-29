using Scenes.Session.Players;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerStorage : MonoBehaviour
{
    [SerializeField] private GameObject PlayerPrefab;
    [SerializeField] private Dictionary<string, Scenes.Session.Players.PlayerStates> _players = new();

    [Inject] private DiContainer _container;

    public void AddPlayer(string PlayerID, string nickName, int CurrentHP, Vector3 postiontion, Vector3 rotation)
    {
        if (_players.ContainsKey(PlayerID)) return;

        var playerObject = _container.InstantiatePrefab(PlayerPrefab);

        playerObject.transform.position = postiontion;
        playerObject.transform.rotation = Quaternion.Euler(rotation);

        if( playerObject.TryGetComponent<Scenes.Session.Players.PlayerStates>(out var playerStates))
        {
            playerStates.nickName = nickName;
            playerStates.playerID = PlayerID;
            playerStates.hp = CurrentHP;
        }


        _players.Add(PlayerID, playerStates);
    }

    public Scenes.Session.Players.PlayerStates GetPlayer(string PlayerID)
    {
        return _players[PlayerID];
    }
}
