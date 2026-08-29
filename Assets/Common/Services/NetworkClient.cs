using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Services.Net.Contracts;
using Common.Services.Net.Interfaces;
using Common.Services.Net.Modules;
using Scenes.SessionRework.Scripts.Network.DTO;
using UnityEngine;
using Zenject;

namespace Editor.Interfaces.Services
{
    public class NetworkClient : INetworkClient,IDisposable
    {
        [Inject] private readonly WebSocketModule _socket;
        [Inject] private readonly UDPModule _udpModule;
        
        
        private readonly Dictionary<PacketType, List<(Delegate Original, Action<IServerUdpPaket> Invoke)>> _packetHandlers = new();
        
        [Inject]
        public void Init()
        {
            _udpModule.OnDataReceived += HandleRawUdpData;
        }
        
        public async Task<bool> TryCreateConnectionTo(string host, int port, Dictionary<string, string> headers = null)
        {
            var uri = $"{host}:{port}";

            try
            {
                var socket = await WebSocketModule.tryCreateConnectionTo(uri, headers);

                await _socket.ReplaceSocketAsync(socket);

                _udpModule.ConnectToServer(host, port);


                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.Message);
                return false;
            }
        }

        public Task SendByTCP(string action)
        {
            if(!_socket.IsConnected)
                throw new Exception("Not connected");
            
            return _socket.Send(action,null);
        }

        public Task SendByTCP(string action, object payload)
        {
            if(!_socket.IsConnected)
                throw new Exception("Not connected");
            
            return _socket.Send(action,payload);
        }

        public Task<WSResponse> SendRequestByTCP(string action)
        {
            if(!_socket.IsConnected)
                throw new Exception("Not connected");
            
            return _socket.SendRequest(action, null);
        }

        public Task<WSResponse> SendRequestByTCP(string action, object payload)
        {
            if(!_socket.IsConnected)
                throw new Exception("Not connected");
            
            return _socket.SendRequest(action, payload);
        }

        public async ValueTask SendByUDP<T>(T packet) where T : struct, IUdpPacket
        {
            if (!_udpModule.IsLinked)
                throw new InvalidOperationException("Not linked");

            byte[] buffer = System.Buffers.ArrayPool<byte>.Shared.Rent(1024); 

            try
            {
                int length = packet.Serialize(buffer.AsSpan());
        
                await _udpModule.SendAsync(new ReadOnlyMemory<byte>(buffer, 0, length));
            }
            finally
            {
                System.Buffers.ArrayPool<byte>.Shared.Return(buffer);
            }
        }

        public void OnUdp<T>(PacketType paketType, Action<T> callback) where T : struct, IServerUdpPaket
        {
            if (!_packetHandlers.TryGetValue(paketType, out var handlers))
            {
                handlers = new List<(Delegate, Action<IServerUdpPaket>)>();
                _packetHandlers.Add(paketType, handlers);
            }
            
            handlers.Add((callback, packet => callback((T)packet)));
        }

        public void OffUdp<T>(PacketType paketType, Action<T> callback) where T : struct, IServerUdpPaket
        {
            if (_packetHandlers.TryGetValue(paketType, out var handlers))
            {
                handlers.RemoveAll(handler => handler.Original == (Delegate)callback);
                
                if (handlers.Count == 0) 
                {
                    _packetHandlers.Remove(paketType);
                }
            }
        }
        
        public void Dispose()
        {
            _packetHandlers.Clear();
            _udpModule.OnDataReceived -= HandleRawUdpData;
        }
        
        private void HandleRawUdpData(ReadOnlyMemory<byte> memoryData)
        {
            var span = memoryData.Span;
            
            if (span.Length < 1 ) return;
            
            PacketType packetType = (PacketType)span[0]; 
            
            var payloadSpan = span.Slice(1);

            if(!_packetHandlers.TryGetValue(packetType, out var handlers))
                return;
            
            switch (packetType)
            {
                case PacketType.PlayerWorldState:
                    var packet = new PlayerStateSnapshot();
                    packet.Deserialize(payloadSpan);
                    
                    foreach (var handler in handlers)
                    {
                        handler.Invoke(packet);
                    }
                    break;
            }
        }
    }
}