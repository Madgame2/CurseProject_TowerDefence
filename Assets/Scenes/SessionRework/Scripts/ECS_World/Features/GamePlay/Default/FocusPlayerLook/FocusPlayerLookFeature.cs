using Scellecs.Morpeh;
using Scenes.SessionRework.Scripts.ECS_World.Features.GamePlay.Default.FocusPlayerLookFeature.Systems;
using Zenject;

namespace Scenes.SessionRework.Scripts.ECS_World.Features.GamePlay.Default.FocusPlayerLookFeature
{
    public static class FocusPlayerLookFeature
    {
        public static void AddFeature(World world, SystemsGroup group, DiContainer container)
        {

            group.AddSystem(container.Instantiate<MakePlayrLookSnapshotSystem>());
        }
    }
}