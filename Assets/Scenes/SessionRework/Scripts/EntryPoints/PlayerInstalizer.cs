using Scellecs.Morpeh;
using Scenes.SessionRework.Scripts.ECS_World.Factories.Interfaces;
using Scenes.SessionRework.Scripts.ECS_World.Factories.Player.Model;
using Scenes.SessionRework.Scripts.GameWorld.Core.interfaces.BackendParams;
using UnityEngine;
using Zenject;

namespace Scenes.SessionRework.Scripts.EntryPoints
{
    public class PlayersInitializer
    {
        private readonly Scellecs.Morpeh.World _ecsWorld;
        private readonly IGetBackendParam _getBackendParam;
        private readonly IPlayerFactory _playerFactory;
        
        public PlayersInitializer(
            Scellecs.Morpeh.World ecsWorld, 
            IGetBackendParam getBackendParam, 
            IPlayerFactory playerFactory)
        {
            _ecsWorld = ecsWorld;
            _getBackendParam = getBackendParam;
            _playerFactory = playerFactory;
        }
        public void Initialize()
        {
            CreatePlayersEntities(_ecsWorld);
        }
        
        private void CreatePlayersEntities(Scellecs.Morpeh.World ecsWorld)
        {
            var players = _getBackendParam.PlayersArray;
            
            foreach (var player in players)
            {
                _playerFactory.CreatePlayer(new PlayerData{
                    PlayerId = player.PlayerId,
                    ObjectId = player.ObjectId,
                    Position = new Vector3(player.Position.X, player.Position.Y, player.Position.Z),
                    IsPlaying = player.IsPlaying,
                });
            }
        }
    }
}