using System;
using System.Collections.Generic;
using Common.Services.Net.Contracts;

namespace Common.Services.Net.Services.PacketProcessor
{
    public class PacketProcessor<T> : IPacketProcessor where T : IServerUdpPaket
    {
        private readonly List<Action<T>> _handlers = new();
        
        
        public void Process(ReadOnlySpan<byte> payload)
        {
            T packet = default; 
            
            packet.Deserialize(payload);
            
            foreach (var handler in _handlers)
            {
                handler.Invoke(packet); 
            }
            
            packet.Release();
        }

        public void AddHandler(Delegate handler) => _handlers.Add((Action<T>)handler);
        
        public void RemoveHandler(Delegate handler) => _handlers.Remove((Action<T>)handler);
    }
}