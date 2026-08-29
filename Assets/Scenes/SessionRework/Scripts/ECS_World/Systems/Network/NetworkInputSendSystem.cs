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

namespace Scenes.SessionRework.Scripts.ECS_World.Systems.Network
{
    public class NetworkInputSendSystem: IFixedSystem
    {
        public World World { get; set; }
        
        private Filter _playerFilter;
        private Filter _simulationFilter;
        
        private Stash<MoveInputHistoryComponent> _historyStash;
        private Stash<ClientSimulationComponent> _simStash;
        
        private INetworkClient _networkClient;
        private readonly IGetBackendParam _backendParam;
        
        private const float SendRate = 20f;
        private readonly float _sendInterval = 1f / SendRate;
        private float _timer;

        public NetworkInputSendSystem(World world, INetworkClient newNetworkClient, IGetBackendParam backendParam)
        {
            World = world;
            _networkClient = newNetworkClient;
            _backendParam = backendParam;
        }
        
        public void OnAwake()
        {
            _playerFilter = World.Filter
                .With<PlayerComponent>()
                .With<MoveInputHistoryComponent>()
                .Build();


            _simulationFilter = World.Filter
                .With<ClientSimulationComponent>()
                .Build();

            _historyStash = World.GetStash<MoveInputHistoryComponent>();
            _simStash = World.GetStash<ClientSimulationComponent>();
        }
        
        public void OnUpdate(float deltaTime)
        {
            _timer += deltaTime;
            
            if (_timer < _sendInterval) return;
            
            _timer -= _sendInterval;
            
            var simEntity = _simulationFilter.First();
            var currentTick = _simStash.Get(simEntity).Tick;
            
            foreach (var entity in _playerFilter)
            {
                ref var history = ref _historyStash.Get(entity);
                
                var packet = new ClientInputPacket
                {
                    UdpToken = _backendParam.UdpToken,
                    LastTick = currentTick,
                    Inputs = FlattenRingBuffer(history.Buffer, history.CurrentIndex)
                };

                _networkClient.SendByUDP(packet);
            }
        }
        
        public void Dispose()
        {
        }
        
        private MoveInputCommand[] FlattenRingBuffer(MoveInputCommand[] buffer, int headIndex)
        {
            var result = new MoveInputCommand[buffer.Length];
            for (int i = 0; i < buffer.Length; i++)
            {
                int index = (headIndex + i) % buffer.Length;
                result[i] = buffer[index];
            }
            return result;
        }
    }
}