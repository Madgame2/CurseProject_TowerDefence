using Scellecs.Morpeh;
using Scenes.SessionRework.Scripts.ECS_World.Features.GamePlay.Default.AlternativeAction.Systems;
using Scenes.SessionRework.Scripts.ECS_World.Features.GamePlay.Default.PrimaryAction.Systems;
using Zenject;

namespace Scenes.SessionRework.Scripts.ECS_World.Features.GamePlay.Default.PrimaryAction
{
    public class PrimaryActionFeature
    {
        public static void AddFeature(World world, SystemsGroup group, DiContainer container)
        {
            AddSystems(world, group, container);
        }

        private static void AddSystems(World world, SystemsGroup group, DiContainer container)
        {
            group.AddSystem(container.Instantiate<AddToPlayerComponentsSystem>());
            group.AddSystem(container.Instantiate<PrimaryActionsInputSystem>());
            group.AddSystem(container.Instantiate<PrimaryActionSystem>());
            group.AddSystem(container.Instantiate<PrimaryActionMakeSnapshotSystem>());
        }
    }
}