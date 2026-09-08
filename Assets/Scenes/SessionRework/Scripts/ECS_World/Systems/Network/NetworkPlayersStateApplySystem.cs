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
    public class NetworkPlayersStateApplySystem: IFixedSystem
    {
        public World World { get; set; }
        
        private readonly INetworkClient _networkClient;

        private Stash<IDComponent> _idComponentStash;
        private Stash<PlayerComponent> _playerComponentStash;
        private Stash<SetPositionRequest> _setPositionRequestStash;
        private Stash<SetVelocityRequest> _setVelocityRequestStash;
        
        private Filter _filter;

        private uint _lastProcesedTick = 0;

        public NetworkPlayersStateApplySystem(World world, INetworkClient networkClient)
        {
            World = world;
            _networkClient = networkClient;
        }
        
        public void OnAwake()
        {
            _idComponentStash = World.GetStash<IDComponent>();
            _playerComponentStash = World.GetStash<PlayerComponent>();
            _setPositionRequestStash = World.GetStash<SetPositionRequest>();
            _setVelocityRequestStash =  World.GetStash<SetVelocityRequest>();
            
            _filter = World.Filter.With<IDComponent>().With<PlayerComponent>().Build();
            
            _networkClient.OnUdp<PlayerStateSnapshot>(PacketType.PlayerWorldState, EnqueueNewPlayerState);
        }
        
        public void OnUpdate(float deltaTime)
        {
            
        }
        
        public void Dispose()
        {
            _networkClient.OffUdp<PlayerStateSnapshot>(PacketType.PlayerWorldState, EnqueueNewPlayerState);

        }

        private void EnqueueNewPlayerState(PlayerStateSnapshot message)
        {
            foreach (var playerState in message.PlayersState)
            {
                if (_lastProcesedTick > message.ServerTick)
                    break;
                
                _lastProcesedTick = message.ServerTick;
                foreach (var player in _filter)
                {
                    ref var objectId = ref _idComponentStash.Get(player).NetId;

                    if (playerState.ObjectId != objectId)
                        continue;
                    
                    _setPositionRequestStash.Set(player, new SetPositionRequest
                    {
                        NewPosition = new Vector3(playerState.Position.x, playerState.Position.y, playerState.Position.z),
                    });
                    
                    _setVelocityRequestStash.Set(player, new SetVelocityRequest
                    {
                        NewVelocity =  new Vector3(playerState.Velocity.x, playerState.Velocity.y, playerState.Velocity.z),
                    });
                }
            }
        }
    }
}