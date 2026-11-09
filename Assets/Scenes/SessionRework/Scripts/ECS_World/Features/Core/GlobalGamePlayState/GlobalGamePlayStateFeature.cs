using Scellecs.Morpeh;
using Scenes.SessionRework.Scripts.ECS_World.Features.Core.GlobalGamePlayState.Components;
using Scenes.SessionRework.Scripts.ECS_World.Features.Core.GlobalGamePlayState.Meta;
using Scenes.SessionRework.Scripts.ECS_World.Features.Core.GlobalGamePlayState.Systems;
using Zenject;

namespace Scenes.SessionRework.Scripts.ECS_World.Features.Core.GlobalGamePlayState
{
    public static class GlobalGamePlayStateFeature
    {
        public static void AddFeature(World world, SystemsGroup updateGroup, DiContainer container)
        {
            CreateEntity(world, updateGroup, container);
            AddSystems(world, updateGroup, container);
        }

        private static void AddSystems(World world, SystemsGroup updateGroup, DiContainer container)
        {
            updateGroup.AddSystem(container.Instantiate<SetGamePlayStateToDefaultSystem>());
        }

        private static void CreateEntity(World world, SystemsGroup updateGroup, DiContainer container)
        {
            var globalGamePlayEntity = world.CreateEntity();
            
            world.GetStash<GlobalGamePlayTag>().Set(globalGamePlayEntity);
            world.GetStash<GamePlayStateComponent>()
                .Set(globalGamePlayEntity, new GamePlayStateComponent{GameplayState = GameplayState.Default});
        }
    }
}