using Scellecs.Morpeh;
using Scenes.SessionRework.Scripts.ECS_World.Components.Players;
using Scenes.SessionRework.Scripts.ECS_World.Features.Core.Input.Compoennts;
using Scenes.SessionRework.Scripts.ECS_World.Systems.Input;
using Zenject;

namespace Scenes.SessionRework.Scripts.ECS_World.Features.Core.Input
{
    public static class InputFeature
    {
        public static void AddFeature(World world, SystemsGroup group, DiContainer container)
        {
            AddComponents(world, container);
            AddSystems(world, group, container);
        }

        private static void AddComponents(World world, DiContainer container)
        {
            var filter = world.Filter.With<MyPlayerComponent>().Build();
            var alternativeActionStash = world.GetStash<AlternativeActionComponent>();
            
            foreach (var playerEntity in filter)
            {
                alternativeActionStash.Set(playerEntity);
            }
        }

        private static void AddSystems(World world, SystemsGroup group, DiContainer container)
        {
            group.AddSystem(container.Instantiate<MovementInputSystem>());
            group.AddSystem(container.Instantiate<JumpInputSystem>());
            group.AddSystem(container.Instantiate<AlternativeFireActionSystem>());
        }
    }
}