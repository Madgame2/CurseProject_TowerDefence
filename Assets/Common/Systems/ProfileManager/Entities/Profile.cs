using Newtonsoft.Json;
using UnityEngine;

namespace Common.systems.ProfileSystem.Entities
{
    public class Profile
    {
        [JsonProperty("id")]
        public string UserId;

        [JsonProperty("nickname")]
        public string ProfileName;
    }
}