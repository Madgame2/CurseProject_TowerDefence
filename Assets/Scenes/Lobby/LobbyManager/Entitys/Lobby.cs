using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

namespace Scenes.Lobby.Entities
{
    public class Lobby
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("users")]
        public List<string> Users { get; set; } = new List<string>();

        [JsonProperty("host")]
        public string Host { get; set; }

        [JsonProperty("maxSize")]
        public int MaxSize { get; set; } = 4;

        [JsonIgnore]
        public bool IsFull => Users.Count >= MaxSize;

        // Пустой конструктор нужен для JsonSerializer
        public Lobby() { }

        public Lobby(string id, string host, int? maxSize = null)
        {
            Id = id;
            Host = host;
            Users = new List<string> { host };
            MaxSize = maxSize ?? 4;
        }
    }
}
