using System.Buffers;
using Common.Services.Net.Interfaces;
using Common.Services.Net.Modules;
using Scellecs.Morpeh;
using Scenes.SessionRework.Scripts.ECS_World.Components.Addition;
using Scenes.SessionRework.Scripts.ECS_World.Components.Movement;
using Scenes.SessionRework.Scripts.ECS_World.Components.Players;
using Scenes.SessionRework.Scripts.ECS_World.Components.Simulation;
using Scenes.SessionRework.Scripts.ECS_World.Systems.Input;
using Scenes.SessionRework.Scripts.GameWorld.Core.interfaces.BackendParams;
using Scenes.SessionRework.Scripts.Network.DTO;
using UnityEngine;
using PlayerStateSnapshot = Scenes.SessionRework.Scripts.ECS_World.Components.Addition.PlayerStateSnapshot;

namespace Scenes.SessionRework.Scripts.ECS_World.Systems.Network
{
    public class NetworkStateSendSystem: IFixedSystem
    {
        public World World { get; set; }
        
        private Filter _historyFilter;
        private Filter _simulationFilter;
        
        private Stash<PlayerStateHistoryComponent> _historyStash;
        private Stash<ClientSimulationComponent> _simStash;
        
        private INetworkClient _networkClient;
        private readonly IGetBackendParam _backendParam;
        
        private const float SendRate = 20f;
        private readonly float _sendInterval = 1f / SendRate;
        private float _timer;

        public NetworkStateSendSystem(World world, INetworkClient newNetworkClient, IGetBackendParam backendParam)
        {
            World = world;
            _networkClient = newNetworkClient;
            _backendParam = backendParam;
        }
        
        public void OnAwake()
        {
            _historyFilter = World.Filter
                .With<PlayerStateHistoryComponent>()
                .Build();


            _simulationFilter = World.Filter
                .With<ClientSimulationComponent>()
                .Build();

            _historyStash = World.GetStash<PlayerStateHistoryComponent>();
            _simStash = World.GetStash<ClientSimulationComponent>();
        }
        
        public void OnUpdate(float deltaTime)
        {
            _timer += deltaTime;

            if (_timer < _sendInterval)
                return;

            _timer -= _sendInterval;

            var simEntity = _simulationFilter.First();
            var currentTick = _simStash.Get(simEntity).Tick;

            foreach (var entity in _historyFilter)
            {
                ref var history = ref _historyStash.Get(entity);

                var inputs = ArrayPool<PlayerStateSnapshot>.Shared.Rent(
                    history.Buffer.Length);

                try
                {
                    FlattenRingBuffer(
                        history.Buffer,
                        history.CurrentIndex,
                        inputs);

                    var packet = new ClientInputPacket
                    {
                        UdpToken = _backendParam.UdpToken,
                        LastTick = currentTick,
                        Inputs = inputs
                    };

                    //Debug.Log(packet);
                    
                    _networkClient.SendByUDP(packet);
                }
                finally
                {
                    ArrayPool<PlayerStateSnapshot>.Shared.Return(inputs);
                }
            }
        }
        
        public void Dispose()
        {
        }
        
        private static void FlattenRingBuffer(
            PlayerStateSnapshot[] buffer,
            int headIndex,
            PlayerStateSnapshot[] result)
        {
            for (int i = 0; i < buffer.Length; i++)
            {
                int index = (headIndex + i) % buffer.Length;
                result[i] = buffer[index];
            }
        }
    }
}