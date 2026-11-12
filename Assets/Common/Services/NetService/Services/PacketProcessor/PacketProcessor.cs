using System;
using System.Collections.Generic;
using Common.Services.Net.Contracts;
using UnityEngine;

namespace Common.Services.Net.Services.PacketProcessor
{
    public class PacketProcessor<T> : IPacketProcessor where T : IServerUdpPaket
    {
        private readonly List<Action<T>> _handlers = new();
        
        
        public void Process(ReadOnlySpan<byte> payload)
        {
            T packet = default;
            try
            {
                packet.Deserialize(payload);

                foreach (var handler in _handlers)
                {
                    handler.Invoke(packet);
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
            finally
            {
                packet.Release();
            }
        }

        public void AddHandler(Delegate handler) => _handlers.Add((Action<T>)handler);
        
        public void RemoveHandler(Delegate handler) => _handlers.Remove((Action<T>)handler);
    }
}