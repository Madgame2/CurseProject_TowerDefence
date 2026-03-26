using Common.Services.Net.Modules;
using System.Net.Cache;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace Common.Services.Net.Services
{
    public class AuthService
    {
        private HttpModule _httpModule;
        private readonly string _baseUrl;

        public AuthService(HttpModule httpModule, string baseUrl)
        {
            _httpModule = httpModule;
            _baseUrl = baseUrl;
        }

        public async Task<HttpResponse> RegisterPlayer(string nickname, string email, string password)
        {
            var json = $"{{\"nickname\":\"{nickname}\",\"email\":\"{email}\",\"password\":\"{password}\"}}";
            return await _httpModule.PostJsonAsync($"{_baseUrl}/profile/register", json);
        }

        public async Task<HttpResponse> AuthByUserInfo(string email, string password)
        {
            var json = $"{{\"email\":\"{email}\",\"password\":\"{password}\"}}";
            return await _httpModule.PostJsonAsync($"{_baseUrl}/profile/login", json);
        }

        public async Task<HttpResponse> TryDeleteUnconfUser(string email)
        {
            return await _httpModule.DeleteJsonAsync($"{_baseUrl}/profile/UnregUser?email={email}");
        }
    }


    public static partial class NetServiceExtensions
    {
        public static AuthService CreateAuthService(this NetService netService)
        {
            return new AuthService(netService._httpModule, netService.BaseUrl);
        }
    }
} 