using Common.Services.Net.Contracts;
using Common.Services.Net.Interfaces;
using Scellecs.Morpeh;
using Scenes.SessionRework.Scripts.ECS_World.Components.Common;
using Scenes.SessionRework.Scripts.ECS_World.Components.Players;
using Scenes.SessionRework.Scripts.ECS_World.Components.Simulation.Requests;
using Scenes.SessionRework.Scripts.Network.DTO;
using UnityEngine;

namespace Scenes.SessionRework.Scripts.ECS_World.Systems.Network
{
    public class NetworkPlayerPositionApplySystem: IFixedSystem
    {
        public World World { get; set; }
        
        private readonly INetworkClient _networkClient;

        private Stash<IDComponent> _idComponentStash;
        private Stash<PlayerComponent> _playerComponentStash;
        private Stash<SetPositionRequest> _setPositionRequestStash;
        
        private Filter _filter;

        private uint _lastProcesedTick = 0;

        public NetworkPlayerPositionApplySystem(World world, INetworkClient networkClient)
        {
            World = world;
            _networkClient = networkClient;
        }
        
        public void OnAwake()
        {
            _idComponentStash = World.GetStash<IDComponent>();
            _playerComponentStash = World.GetStash<PlayerComponent>();
            _setPositionRequestStash = World.GetStash<SetPositionRequest>();

            _filter = World.Filter.With<IDComponent>().With<PlayerComponent>().Build();
            
            _networkClient.OnUdp<PlayerStateSnapshot>(PacketType.PlayerWorldState, EnqueueNewPlayerPosition);
        }
        
        public void OnUpdate(float deltaTime)
        {
            
        }
        
        public void Dispose()
        {
            _networkClient.OffUdp<PlayerStateSnapshot>(PacketType.PlayerWorldState, EnqueueNewPlayerPosition);

        }

        private void EnqueueNewPlayerPosition(PlayerStateSnapshot message)
        {
            foreach (var player in _filter)
            {
                ref var playerId = ref _playerComponentStash.Get(player).PlayerId;
                
                if(message.UserId != playerId)
                    continue;
                
                if(_lastProcesedTick>message.ServerTick)
                    continue;
                
                _lastProcesedTick =  message.ServerTick;
                _setPositionRequestStash.Set(player, new SetPositionRequest
                {
                    NewPosition = new Vector3(message.Position.x, message.Position.y, message.Position.z),
                });
            }
        }
    }
}