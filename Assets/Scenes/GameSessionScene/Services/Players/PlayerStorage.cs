using Scenes.Session.Players;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class PlayerStorage : MonoBehaviour
{
    [SerializeField] private GameObject PlayerPrefab;
    [SerializeField] private Dictionary<string, Scenes.Session.Players.PlayerStates> _players = new();

    [Inject] private DiContainer _container;

    public bool HasPlayer(string playerName)
    {
        return _players.ContainsKey(playerName);
    }
    public void AddPlayer(string PlayerID, string nickName, int CurrentHP, Vector3 postiontion, Vector3 rotation)
    {
        if (_players.ContainsKey(PlayerID)) return;

        var playerObject = _container.InstantiatePrefab(PlayerPrefab);

        playerObject.transform.position = postiontion*10;
        playerObject.transform.rotation = Quaternion.Euler(rotation);

        if( playerObject.TryGetComponent<Scenes.Session.Players.PlayerStates>(out var playerStates))
        {
            playerStates.nickName = nickName;
            playerStates.playerID = PlayerID;
            playerStates.hp = CurrentHP;
        }


        _players.Add(PlayerID, playerStates);
    }

    public void RemovePlayer(string playerID)
    {
        _players.Remove(playerID);
    }

    public GameObject GetByPlayerGameObjectId(string playerID)
    {
        if (!_players.TryGetValue(playerID, out var player) || !player)
        {
            _players.Remove(playerID);
            return null;
        }

        if (!player)
        {
            _players.Remove(playerID);
            return null;
        }

        return player.gameObject;
    }

    public Scenes.Session.Players.PlayerStates GetPlayer(string playerID)
    {
        if (!_players.TryGetValue(playerID, out var player) || !player)
        {
            _players.Remove(playerID);
            return null;
        }

        if (!player)
        {
            _players.Remove(playerID);
            return null;
        }

        return player;
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _players.Clear();
    }

}
