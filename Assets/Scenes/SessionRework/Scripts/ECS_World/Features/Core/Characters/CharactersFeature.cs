using Scellecs.Morpeh;
using Scenes.SessionRework.Scripts.ECS_World.Features.Core.Characters.Systems;
using Zenject;

namespace Scenes.SessionRework.Scripts.ECS_World.Features.Core.Characters
{
    public static class CharactersFeature
    {
        public static void AddFeature(World world,
            SystemsGroup systemsGroup,
            DiContainer container)
        {
            AddSystems(world, systemsGroup, container);
        }

        private static void AddSystems(World world, SystemsGroup systemsGroup, DiContainer container)
        {
            systemsGroup.AddSystem(container.Instantiate<AddCharacterControllerSystem>());
            systemsGroup.AddSystem(container.Instantiate<SyncCharacterWithControllerSystem>());
        }
    }
}