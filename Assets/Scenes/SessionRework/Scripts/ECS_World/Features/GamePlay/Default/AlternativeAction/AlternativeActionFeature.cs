using Scellecs.Morpeh;
using Scenes.SessionRework.Scripts.ECS_World.Features.GamePlay.Default.AlternativeAction.Systems;
using Zenject;

namespace Scenes.SessionRework.Scripts.ECS_World.Features.GamePlay.Default.AlternativeAction
{
    public static class AlternativeActionFeature
    {
        public static void AddFeature(World world, SystemsGroup group, DiContainer container)
        {
            AddSystems(world, group, container);
        }

        private static void AddSystems(World world, SystemsGroup group, DiContainer container)
        {
            group.AddSystem(container.Instantiate<AlternativeFireActionSystem>());
        }
    }
}