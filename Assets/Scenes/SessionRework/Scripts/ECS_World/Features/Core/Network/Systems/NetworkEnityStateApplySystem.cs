using Common.Services.Net.Contracts;
using Common.Services.Net.Interfaces;
using Scellecs.Morpeh;
using Scenes.SessionRework.Scripts.ECS_World.Components.Common;
using Scenes.SessionRework.Scripts.ECS_World.Components.Geometry;
using Scenes.SessionRework.Scripts.ECS_World.Features.Core.Projectiles.Components;
using Scenes.SessionRework.Scripts.Network.DTO;
using Scenes.SessionRework.Scripts.Network.DTO.EntityStates;
using Scenes.SessionRework.Scripts.Network.DTO.EntityStates.State.Projectile;
using Scenes.SessionRework.Scripts.Services.Replication.Models;
using UnityEngine;

namespace Scenes.SessionRework.Scripts.ECS_World.Features.Systems
{
    public class NetworkEnityStateApplySystem: IFixedSystem
    {
        public World World { get; set; }
        
        private INetworkClient _networkClient;
        private Filter _projectileFilter;
        private Stash<PositionComponent> _positionStash;
        private Stash<RotationComponent> _rotationStash;
        private Stash<IDComponent> _idStash;

        public NetworkEnityStateApplySystem(World world, INetworkClient networkClient)
        {
            World = world;
            _networkClient = networkClient;
        }
        
        public void OnAwake()
        {
            _networkClient.OnUdp<EntitesStatSnapshot>(PacketType.EntityStateSnapshot, ProcessPacket);
            
            _rotationStash = World.GetStash<RotationComponent>();
            _positionStash = World.GetStash<PositionComponent>();
            _idStash  = World.GetStash<IDComponent>();

            _projectileFilter = World.Filter
                .With<IDComponent>()
                .With<ProjectileTag>()
                .With<PositionComponent>()
                .With<RotationComponent>()
                .Build();
        }

        public void OnUpdate(float deltaTime)
        {
        }
        
        public void Dispose()
        {
            _networkClient.OffUdp<EntitesStatSnapshot>(PacketType.EntityStateSnapshot, ProcessPacket);
        }
        
        private void ProcessPacket(EntitesStatSnapshot obj)
        {
            if(obj.entityStates.Length<=0) 
                return;
            

            for (int i = 0; i < obj.ValidEntitiesCount; i++) 
            {
                var entityState =  obj.entityStates[i];
                
                switch (entityState.objectType)
                {
                    case ObjectType.Projectile:
                        ProcessprojectileState(entityState.projectile);
                        break;
                    
                    default:
                        Debug.LogError("unsuported type in paket :" + entityState.objectType);
                        break;
                }
            }
        }

        private void ProcessprojectileState(ProjectileState entityStateProjectile)
        {
            foreach (var entity in _projectileFilter)
            {
                ref var idComponent = ref _idStash.Get(entity);
                
                if(idComponent.NetId !=entityStateProjectile.ObjectId)
                    continue;
                
                ref var rotationComponent =ref _rotationStash.Get(entity);
                ref var positionComponent = ref _positionStash.Get(entity);
                
                positionComponent.Position = entityStateProjectile.Position;
                rotationComponent.Rotation = entityStateProjectile.Rotation;
            }
        }
    }
}