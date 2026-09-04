using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Services.Net.Contracts;
using Common.Services.Net.Interfaces;
using Common.Services.Net.Modules;
using Common.Services.Net.Services.PacketProcessor;
using Scenes.SessionRework.Scripts.Network.DTO;
using UnityEngine;
using Zenject;

namespace Editor.Interfaces.Services
{
    public class NetworkClient : INetworkClient,IDisposable
    {
        [Inject] private readonly WebSocketModule _socket;
        [Inject] private readonly UDPModule _udpModule;
        
        
        private readonly Dictionary<PacketType, IPacketProcessor> _processors = new();        
        [Inject]
        public void Init()
        {
            RegisterProcessor<PlayerStateSnapshot>(PacketType.PlayerWorldState);
            
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
            if (_processors.TryGetValue(paketType, out var processor) && processor is PacketProcessor<T> typedProcessor)
            {
                typedProcessor.AddHandler(callback);
            }
            else
            {
                Debug.LogError($"[NetworkClient] Процессор для пакета {paketType} не зарегистрирован или тип не совпадает!");
            }
        }

        public void OffUdp<T>(PacketType paketType, Action<T> callback) where T : struct, IServerUdpPaket
        {
            if (_processors.TryGetValue(paketType, out var processor) && processor is PacketProcessor<T> typedProcessor)
            {
                typedProcessor.RemoveHandler(callback);
            }
        }
        
        public void Dispose()
        {
            _processors.Clear();
            _udpModule.OnDataReceived -= HandleRawUdpData;
        }
        
        private void HandleRawUdpData(ReadOnlyMemory<byte> memoryData)
        {
            var span = memoryData.Span;
            
            if (span.Length < 1) return;
            
            PacketType packetType = (PacketType)span[0]; 
            var payloadSpan = span.Slice(1);

            if (_processors.TryGetValue(packetType, out var processor))
            {
                processor.Process(payloadSpan);
            }
        }
        
        private void RegisterProcessor<T>(PacketType type) where T : IServerUdpPaket
        {
            _processors[type] = new PacketProcessor<T>();
        }
    }
}