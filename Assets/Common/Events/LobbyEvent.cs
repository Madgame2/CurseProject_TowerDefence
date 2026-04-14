using UnityEngine;


namespace Common.Events
{
    public class LobbyEvent
    {
        public string type { get; set; }
        public string lobbyId { get; set; }
        public Scenes.Lobby.Entities.Lobby lobby { get; set; }
    }
}
