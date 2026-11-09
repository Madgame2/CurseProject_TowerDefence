using System;
using Scellecs.Morpeh;
using Scenes.SessionRework.Scripts.ECS_World.Components.Players;
using Scenes.SessionRework.Scripts.ECS_World.Features.Core.Characters.Meta;
using Scenes.SessionRework.Scripts.ECS_World.Features.Core.Meta;
using Zenject;

namespace Scenes.SessionRework.Characters.Skadi.CharacterController.Factory
{
    public class SkadiControllerFactoryModule : ICharacterControllerFactoryModule
    {
        private readonly DiContainer _container;

        public SkadiControllerFactoryModule(DiContainer container)
        {
            _container = container;
        }

        public CharacterControllerBase CreateController(
            World world,
            Entity playerEntity)
        {
            ref var playerComponent = ref world
                .GetStash<PlayerComponent>()
                .Get(playerEntity, out bool isExist);

            if (!isExist)
                throw new ArgumentException(
                    $"PlayerComponent not found for entity {playerEntity}");

            return _container.Instantiate<SkadiController>(
                new object[] { playerComponent.PlayerView });
        }
    }
}