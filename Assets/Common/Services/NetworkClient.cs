using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Services.Net.Contracts;
using Common.Services.Net.Flags;
using Common.Services.Net.Interfaces;
using Common.Services.Net.Modules;
using Common.Services.Net.Modules.Extra;
using Common.Services.Net.Services.PacketProcessor;
using Scenes.SessionRework.Scripts.Network.DTO;
using UnityEngine;
using Zenject;
using System.Buffers;
using System.Threading;
using Scenes.SessionRework.Scripts.GameWorld.Core.interfaces.BackendParams;

namespace Editor.Interfaces.Services
{
    public class NetworkClient : INetworkClient,IDisposable
    {
        [Inject] private readonly WebSocketModule _socket;
        [Inject] private readonly UDPModule _udpModule;
        
        private readonly ReceivedPacketWindow _receivedPackets = new(1024);
        private readonly Dictionary<PacketType, IPacketProcessor> _processors = new();
        private const int DefaultPort = 443;
        
        private const int TransportHeaderSize = 5;
        
        private long _sequenceId;
        
        
        [Inject]
        public void Init()
        {
            RegisterProcessor<PlayerStateSnapshot>(PacketType.PlayerWorldState);
            RegisterProcessor<ReplicationLogPaket>(PacketType.ReplicationLog);
            
            _udpModule.OnDataReceived += HandleRawUdpData;
        }
        private uint NextSequenceId()
        {
            return unchecked((uint)Interlocked.Increment(ref _sequenceId));
        }
        
        public Task<bool> TryCreateConnectionTo(
            string host,
            int port,
            Dictionary<string, string> headers = null)
        {
            return TryCreateConnectionTo($"{host}:{port}", headers);
        }


        public async Task<bool> TryCreateConnectionTo(
            string address,
            Dictionary<string, string> headers = null)
        {
            try
            {
                if (!address.Contains("://"))
                    address = $"ws://{address}";

                var uri = new Uri(address);

                var port = uri.IsDefaultPort ? DefaultPort : uri.Port;

                var socket = await WebSocketModule.tryCreateConnectionTo(
                    uri, headers);

                await _socket.ReplaceSocketAsync(socket);

                _udpModule.ConnectToServer(uri.Host, port);

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

        public ValueTask SendByUDP<T>(T packet)
            where T : struct, IUdpPacket
        {
            return SendUdpInternalAsync(packet, TransportFlags.None);
        }

        public ValueTask SendReliableByUDP<T>(T packet)
            where T : struct, IUdpPacket
        {
            return SendUdpInternalAsync(packet, TransportFlags.Reliable);
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
            _receivedPackets.Clear();

            _udpModule.OnDataReceived -= HandleRawUdpData;
        }
        
        private void HandleRawUdpData(ReadOnlyMemory<byte> memoryData)
        {
            var span = memoryData.Span;

            if (span.Length < TransportHeaderSize)
                return;

            TransportFlags flags = (TransportFlags)span[0];

            uint sequenceId = BinaryPrimitives.ReadUInt32LittleEndian(
                span.Slice(1, 4));

// ACK не содержит PacketType и Payload.
            if (flags == TransportFlags.Acknowledgement)
                return;

            // Для остальных пакетов нужен хотя бы PacketType.
            if (span.Length < TransportHeaderSize + 1)
                return;
            
            // Обрабатываем служебные пакеты транспортного уровня.
            switch (flags)
            {
                case TransportFlags.None:
                    break;

                case TransportFlags.Reliable:

                    // Подтверждаем получение даже повторного пакета.
                    SendAck(sequenceId);

                    // Повторно прикладной обработчик не вызываем.
                    if (!_receivedPackets.TryMarkReceived(sequenceId))
                        return;

                    break;

                case TransportFlags.Acknowledgement:
                    // ACK не содержит прикладного пакета.
                    // Пока клиент не отправляет Reliable-пакеты,
                    // обрабатывать здесь нечего.
                    return;

                default:
                    return;
            }

            // Переходим к прикладному уровню.
            var applicationSpan = span.Slice(TransportHeaderSize);

            PacketType packetType = (PacketType)applicationSpan[0];

            var payloadSpan = applicationSpan.Slice(1);

            if (_processors.TryGetValue(packetType, out var processor))
            {
                processor.Process(payloadSpan);
            }
        }
        
        private void SendAck(uint sequenceId)
        {
            _ = SendAckInternalAsync(sequenceId);
        }
        
        
        private async ValueTask SendUdpInternalAsync<T>(
            T packet,
            TransportFlags flags)
            where T : struct, IUdpPacket
        {
            if (!_udpModule.IsLinked)
                throw new InvalidOperationException("Not linked");

            byte[] buffer = System.Buffers.ArrayPool<byte>.Shared.Rent(1024 + TransportHeaderSize);

            try
            {
                // Сериализуем прикладной пакет после транспортного заголовка.
                int payloadLength = packet.Serialize(
                    buffer.AsSpan(TransportHeaderSize));

                uint sequenceId = unchecked(
                    (uint)Interlocked.Increment(ref _sequenceId));

                // TransportFlags
                buffer[0] = (byte)flags;

                // SequenceId
                BinaryPrimitives.WriteUInt32LittleEndian(
                    buffer.AsSpan(1, 4),
                    sequenceId);

                int totalLength = TransportHeaderSize + payloadLength;

                await _udpModule.SendAsync(
                    new ReadOnlyMemory<byte>(buffer, 0, totalLength));
            }
            finally
            {
                System.Buffers.ArrayPool<byte>.Shared.Return(buffer);
            }
        }
        
        
        private async Task SendAckInternalAsync(uint sequenceId)
        {
            try
            {
                await SendAckAsync(sequenceId);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[NetworkClient] Failed to send ACK: {ex}");
            }
        }
        private async ValueTask SendAckAsync(uint sequenceId)
        {
            byte[] buffer = System.Buffers.ArrayPool<byte>.Shared.Rent(TransportHeaderSize + AcknowledgePacket.PaketSize);
            var packet = new AcknowledgePacket();
            
            try
            {
                buffer[0] = (byte)TransportFlags.Acknowledgement;

                BinaryPrimitives.WriteUInt32LittleEndian(
                    buffer.AsSpan(1, 4),
                    sequenceId);

                int payloadLength = packet.Serialize(
                    buffer.AsSpan(TransportHeaderSize));
                
                int totalLength = TransportHeaderSize + payloadLength;

                await _udpModule.SendAsync(
                    new ReadOnlyMemory<byte>(buffer, 0, totalLength));
            }
            finally
            {
                System.Buffers.ArrayPool<byte>.Shared.Return(buffer);
            }
        }
        
        private void RegisterProcessor<T>(PacketType type) where T : IServerUdpPaket
        {
            _processors[type] = new PacketProcessor<T>();
        }
    }
}