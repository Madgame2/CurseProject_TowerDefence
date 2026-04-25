using Common.systems.ProfileSystem.Entities;
using Scenes.Lobby.Entities;
using UnityEngine;


namespace Common.Events
{
    public class LobbyEvent
    {
        public string type { get; set; }
        public string lobbyId { get; set; }
        public Scenes.Lobby.Entities.Lobby lobby { get; set; }
        public Player profile { get; set; }
        public  string userId { get; set; }
        public string hostID {  get; set; }
        public string? state { get; set; }
    }
}
