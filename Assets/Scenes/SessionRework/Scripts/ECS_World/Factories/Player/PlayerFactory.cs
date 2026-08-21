using Scellecs.Morpeh;
using Scenes.SessionRework.Scripts.ECS_World.Components.Common;
using Scenes.SessionRework.Scripts.ECS_World.Components.Geometry;
using Scenes.SessionRework.Scripts.ECS_World.Components.Movement;
using Scenes.SessionRework.Scripts.ECS_World.Components.Players;
using Scenes.SessionRework.Scripts.ECS_World.Factories.Interfaces;
using Scenes.SessionRework.Scripts.ECS_World.Factories.Player.Model;
using Scenes.SessionRework.Scripts.GameWorld.Core;
using Scenes.SessionRework.Scripts.Player.View;
using UnityEngine;
using Zenject;

namespace Scenes.SessionRework.Scripts.ECS_World.Factories.Player
{
    public class PlayerFactory : IPlayerFactory
    {
        private readonly Scellecs.Morpeh.World _world;
        [Inject] private PlayerView _playerView;
        
        private readonly Stash<IDComponent> _idStash;
        private readonly Stash<PositionComponent> _positionStash;
        private readonly Stash<PlayerComponent> _playerStash;
        private readonly Stash<UnityViewComponent> _viewStash;
        private readonly Stash<InputComponent> _inputStash;
        //private readonly Stash<HealthComponent> _healthStash;

        public PlayerFactory(World world)
        {
            _world = world;
            
            _idStash = _world.GetStash<IDComponent>();
            _positionStash = _world.GetStash<PositionComponent>();
            _playerStash = _world.GetStash<PlayerComponent>();
            _viewStash = _world.GetStash<UnityViewComponent>(); 
            _inputStash = _world.GetStash<InputComponent>();
            //_healthStash = _world.GetStash<HealthComponent>();
        }
        
        public Scellecs.Morpeh.Entity CreatePlayer(PlayerData data)
        {
            var viewObject = Object.Instantiate(_playerView);
            
            var entity = _world.CreateEntity();
            
            _idStash.Set(entity, new IDComponent { Id = data.Id });
            _positionStash.Set(entity, new PositionComponent { Position = data.Position });
            //_healthStash.Set(entity, new HealthComponent { Health = data.MaxHealth });

            if (data.IsPlaying)
            {
                _playerStash.Set(entity, new PlayerComponent());
            }

            _viewStash.Set(entity, new UnityViewComponent 
            { 
                GameObject = viewObject.gameObject, 
                Transform = viewObject.transform 
            });
            
            _inputStash.Set(entity);
            
            _world.Commit(); 
        
            return entity;
        }
    }
}