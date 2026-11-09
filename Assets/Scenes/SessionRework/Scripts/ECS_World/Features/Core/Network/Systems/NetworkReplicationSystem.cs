using System;
using Common.Services.Net.Contracts;
using Common.Services.Net.Interfaces;
using Scellecs.Morpeh;
using Scenes.SessionRework.Scripts.ECS_World.Components.Common;
using Scenes.SessionRework.Scripts.ECS_World.Components.Players;
using Scenes.SessionRework.Scripts.ECS_World.Factories.Interfaces;
using Scenes.SessionRework.Scripts.ECS_World.Factories.Player.Model;
using Scenes.SessionRework.Scripts.Network.DTO;
using Scenes.SessionRework.Scripts.Services.Replication.Models;
using UnityEngine;
using EventType = Scenes.SessionRework.Scripts.Services.Replication.Events.EventType;

namespace Scenes.SessionRework.Scripts.ECS_World.Features.Systems
{
    public class NetworkReplicationSystem : IFixedSystem
    {
        public World World { get; set; }
        
        private readonly INetworkClient _networkClient;
        private readonly IPlayerFactory _playerFactory;
        
        private Stash<PlayerComponent>  _stash;

        private uint _lastProcesedTick = 0;

        public NetworkReplicationSystem(World world,  INetworkClient networkClient, IPlayerFactory playerFactory)
        {
            World = world;
            _networkClient = networkClient;
            _playerFactory = playerFactory;
        }

        public void OnAwake()
        {
            _stash = World.GetStash<PlayerComponent>();
            
            _networkClient.OnUdp<ReplicationLogPaket>(PacketType.ReplicationLog, ProcessReplicationLog);
        }
        

        public void OnUpdate(float deltaTime)
        {
            
        }
        
        public void Dispose()
        {
            _networkClient.OffUdp<ReplicationLogPaket>(PacketType.ReplicationLog, ProcessReplicationLog);
        }
        
        private bool ExistsObject(uint objectId)
        {
            var filter = World.Filter.With<IDComponent>().Build();

            foreach (var entity in filter)
            {
                if (entity.GetComponent<IDComponent>().NetId == objectId)
                    return true;
            }

            return false;
        }
        
        private void ProcessReplicationLog(ReplicationLogPaket obj)
        {
            if (_lastProcesedTick >= obj.ServerTick)
                return;

            for (int i = 0; i < obj.ValidEntriesCount; i++)
            {
                ref readonly var entry = ref obj.Entries[i];

                if (_lastProcesedTick >= entry.Tick)
                    continue;

                _lastProcesedTick = entry.Tick;

                var serverEvent = entry.Event;

                if (serverEvent.eventType == EventType.SpawnEvent)
                {
                    var spawnEvent = serverEvent.spawnEvent;

                    if (spawnEvent.objectType == ObjectType.Player)
                    {
                        // Проверяем, существует ли объект с таким ObjectId
                        uint objectId = spawnEvent.objectId;
                        
                        if (ExistsObject(objectId))
                            continue;

                        var payload = spawnEvent.payload.playerPayload;

                        try
                        {
                            Debug.Log("СОЗДАЮ");

                            _playerFactory.CreatePlayer(new PlayerData
                            {
                                PlayerId = payload.playerID,
                                ObjectId = objectId,
                                Position = payload.Transforms.Position,
                                IsPlaying = false,
                            });
                        }
                        catch (Exception ex)
                        {
                            Debug.LogError(ex);
                        }
                    }
                }
            }
        }
    }
}