using Scellecs.Morpeh;
using Scenes.SessionRework.Scripts.ECS_World.Components.Addition;
using Scenes.SessionRework.Scripts.ECS_World.Components.Common;
using Scenes.SessionRework.Scripts.ECS_World.Components.Geometry;
using Scenes.SessionRework.Scripts.ECS_World.Components.Movement;
using Scenes.SessionRework.Scripts.ECS_World.Components.Players;
using Scenes.SessionRework.Scripts.ECS_World.Components.Simulation;
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
        private readonly Stash<MoveInputHistoryComponent> _inputHistoryStash;
        private readonly Stash<MyPlayerComponent> _myPlayerStash;
        private readonly Stash<RotationComponent>  _rotationStash;
        private readonly Stash<VelocityComponent> _velocityStash;
        //private readonly Stash<HealthComponent> _healthStash;

        public PlayerFactory(World world)
        {
            _world = world;

            _idStash = _world.GetStash<IDComponent>();
            _positionStash = _world.GetStash<PositionComponent>();
            _playerStash = _world.GetStash<PlayerComponent>();
            _viewStash = _world.GetStash<UnityViewComponent>();
            _inputStash = _world.GetStash<InputComponent>();
            _inputHistoryStash = _world.GetStash<MoveInputHistoryComponent>();
            _myPlayerStash = world.GetStash<MyPlayerComponent>();
            _rotationStash = world.GetStash<RotationComponent>();
            _velocityStash = world.GetStash<VelocityComponent>();
            //_healthStash = _world.GetStash<HealthComponent>();
        }

        public Scellecs.Morpeh.Entity CreatePlayer(PlayerData data)
        {
            var entity = _world.CreateEntity();

            InitializeBaseComponents(entity, data);
            InitializeView(entity,data);

            if (data.IsPlaying)
            {
                InitializePlayerComponents(entity);
            }

            _world.Commit();

            return entity;
        }

        private void InitializeBaseComponents(Scellecs.Morpeh.Entity entity, PlayerData data)
        {
            _idStash.Set(entity, new IDComponent{NetId = data.ObjectId});

            _positionStash.Set(entity, new PositionComponent
            {
                Position = data.Position
            });
            
            _rotationStash.Set(entity, new RotationComponent
            {
                Rotation = Quaternion.identity
            });
            
            _velocityStash.Set(entity);
        }

        private void InitializePlayerComponents(Scellecs.Morpeh.Entity entity)
        {
            _myPlayerStash.Set(entity, new MyPlayerComponent());

            _inputStash.Set(entity);

            _inputHistoryStash.Set(entity,
                new MoveInputHistoryComponent
                {
                    CurrentIndex = 0,
                    Buffer = new MoveInputCommand[15]
                });
        }

        private void InitializeView(Scellecs.Morpeh.Entity entity, PlayerData data)
        {
            var viewObject = Object.Instantiate(_playerView).GetComponent<PlayerView>();

            _playerStash.Set(entity, new PlayerComponent
            {
                PlayerId = data.PlayerId,
                PlayerView = viewObject
            });
            
            _viewStash.Set(entity, new UnityViewComponent
            {
                GameObject = viewObject.gameObject,
                Transform = viewObject.transform,
                Animator =  viewObject.Animator,
            });
            
            //viewObject.LinkEntity(entity);
        }
    }
}