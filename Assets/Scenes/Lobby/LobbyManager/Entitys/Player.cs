using Newtonsoft.Json;
using UnityEngine;

namespace Scenes.Lobby.Entities
{
    public class Player
    {
        [JsonProperty("id")]
        public string PlayerId {  get; set; }

        [JsonProperty("NickName")]
        public string Name { get; set; }

        [JsonProperty("HeaderImage")]
        public string avatarSource {  get; set; }
    }
}
