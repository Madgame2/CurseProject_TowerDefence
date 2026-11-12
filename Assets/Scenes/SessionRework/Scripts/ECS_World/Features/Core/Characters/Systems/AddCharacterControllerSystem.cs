using Scellecs.Morpeh;
using Scenes.SessionRework.Characters.Skadi.CharacterController;
using Scenes.SessionRework.Characters.Skadi.CharacterController.Factory;
using Scenes.SessionRework.Scripts.ECS_World.Components.Players;
using Scenes.SessionRework.Scripts.ECS_World.Features.Core.Characters.Components;
using Scenes.SessionRework.Scripts.ECS_World.Features.Core.Characters.Meta;
using Zenject;

namespace Scenes.SessionRework.Scripts.ECS_World.Features.Core.Characters.Systems
{
    public class AddCharacterControllerSystem:ISystem
    {
        public World World { get; set; }

        private Filter _characterFilter;
        private Stash<CharacterController.PlayerControllerComponent>  _stash;
        
        private ICharacterControllerFactoryModule _skadiControllerFactory;
        
        private DiContainer _container;
        
        public AddCharacterControllerSystem(World world,  DiContainer container)
        {
            World = world;
            _container = container;
        }
        
        public void OnAwake()
        {
            _skadiControllerFactory =  _container.Instantiate<SkadiControllerFactoryModule>();
            
            _characterFilter = World.Filter
                .With<PlayerComponent>()
                .Without<CharacterController.PlayerControllerComponent>()
                .Build();
            
            _stash = World.GetStash<CharacterController.PlayerControllerComponent>();
        }


        public void OnUpdate(float deltaTime)
        {
            foreach (var playerEntity in _characterFilter)
            {
                var playerComponent = World
                    .GetStash<PlayerComponent>()
                    .Get(playerEntity);

                var controller = _skadiControllerFactory.CreateController(World, playerEntity);

                _stash.Set(playerEntity, new CharacterController.PlayerControllerComponent
                {
                    Handler = controller
                });
            }
        }
        
        public void Dispose()
        {
        }
    }
}