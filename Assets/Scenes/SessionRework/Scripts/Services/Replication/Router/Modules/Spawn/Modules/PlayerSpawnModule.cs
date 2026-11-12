using Scellecs.Morpeh;
using Scenes.SessionRework.Scripts.ECS_World.Components.Common;
using Scenes.SessionRework.Scripts.ECS_World.Components.Players;
using Scenes.SessionRework.Scripts.ECS_World.Factories.Interfaces;
using Scenes.SessionRework.Scripts.ECS_World.Factories.Player.Model;
using Scenes.SessionRework.Scripts.Services.Replication.Events.Spawn.Payload;
using Scenes.SessionRework.Scripts.Services.Replication.Models;
using Scenes.SessionRework.Scripts.Services.Replication.Router.Modules.Spawn.Modules.Base;
using UnityEngine.LightTransport;

namespace Scenes.SessionRework.Scripts.Services.Replication.Router.Modules.Spawn.Modules
{
    public class PlayerSpawnModule :ISpawnModule
    {
        public ObjectType ObjectType => ObjectType.Player;
        private readonly IPlayerFactory _playerFactory;

        private World _world;
        private Filter _playerFilter;
        private Stash<IDComponent> _idStash;
        
        public PlayerSpawnModule(IPlayerFactory playerFactory, World world)
        {
            _world = world;
            _playerFactory = playerFactory;
            
            _idStash = world.GetStash<IDComponent>();
        }
        
        public void Spawn(uint objectId, SpawnPayload spawnPayload)
        {
            var payload = spawnPayload.playerPayload;
            
            if(existPlayer(objectId))
                return;
            
            _playerFactory.CreatePlayer(new PlayerData
            {
                PlayerId = payload.playerID,
                ObjectId = objectId,
                Position = payload.Transforms.Position,
                IsPlaying = false,
            });
        }

        private bool existPlayer(uint objectId)
        {
            if (_playerFilter == null)
            {
                _playerFilter = _world.Filter
                    .With<IDComponent>()
                    .With<PlayerComponent>()
                    .Build();
            }

            foreach (var player in _playerFilter)
            {
                if(_idStash.Get(player).NetId == objectId)
                    return true;
            }
            
            return false;
        }
    }
}