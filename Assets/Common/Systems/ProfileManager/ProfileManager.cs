using Common.Services.Net.Modules;
using Common.systems.ProfileSystem.Entities;
using Newtonsoft.Json.Linq;
using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Common.systems.ProfileSystem
{
    public class ProfileManager
    {
        private Profile _myProfile = null;
        private WebSocketModule _socket;

        public event Action onServerError;
        public event Action onPlayerNotFound;
        public event Action onTimeOut;

        public event Action onProfileUpdated;


        public Profile Profile { get => _myProfile;
            private set
            {
                _myProfile = value;
                onProfileUpdated?.Invoke();
            }
        }

        public ProfileManager(WebSocketModule socket)
        {
            _socket = socket;
        }

        public async Task Init()
        {
            using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(25)))
            {
                CancellationToken token = cts.Token;

                try
                {
                    WSResponse response = await _socket.SendRequest("GetProfile", new { }, token);
                    Debug.Log(response.Code);

                    Profile = HandleResponce(response);
                    Debug.Log(Profile.UserId);
                }
                catch (OperationCanceledException)
                {
                    onTimeOut.Invoke();
                    return;
                }
            }
            return;


            Profile HandleResponce(WSResponse response)
            {
                switch (response.Code)
                {
                    case 200:
                        {
                            return (response.Data as JObject)?.ToObject<Profile>();
                        }
                    case 404:
                        {
                            onPlayerNotFound?.Invoke();
                        }
                        break;
                    case 500:
                    default:
                        {
                            onServerError?.Invoke();
                        }
                        break;
                }

                return null;
            }
        }
    }
}
