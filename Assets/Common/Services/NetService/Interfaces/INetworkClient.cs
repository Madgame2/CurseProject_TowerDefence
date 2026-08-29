using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Services.Net.Contracts;
using Common.Services.Net.Modules;

namespace Common.Services.Net.Interfaces
{
    public interface INetworkClient
    {
        Task<bool> TryCreateConnectionTo(string host, int port, Dictionary<string, string> headers = null);

        Task SendByTCP(string action);
        Task SendByTCP(string action, object payload);
        
        Task<WSResponse> SendRequestByTCP(string action);
        Task<WSResponse> SendRequestByTCP(string action, object payload);

        ValueTask SendByUDP<T>(T packet) where T : struct, IUdpPacket;
        void OnUdp<T>(PacketType paketType, Action<T> callback) where T : struct, IServerUdpPaket;
        void OffUdp<T>(PacketType paketType, Action<T> callback) where T : struct, IServerUdpPaket;
    }
}