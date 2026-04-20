using UnityEngine;

namespace Common.Services.Net.Modules
{
    public class SessionServerInfo
    {
        public string host { get; set; }
        public int port { get; set; }
        public string sessionId {  get; set; }
        public string passToken { get; set; }
        public string lobbyId { get; set; }
    }
}