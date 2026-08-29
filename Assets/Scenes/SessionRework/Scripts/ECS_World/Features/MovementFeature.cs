using Scellecs.Morpeh;
using Scenes.SessionRework.Scripts.ECS_World.Systems;
using Scenes.SessionRework.Scripts.ECS_World.Systems.Input;
using Zenject;

namespace Scenes.SessionRework.Scripts.ECS_World.Features
{
    public static class MovementFeature
    {
        public static void AddFeature(World world,SystemsGroup group, DiContainer container)
        {
            group.AddSystem(container.Instantiate<ApplyNewPlayerPosition>());
            group.AddSystem(container.Instantiate<TransformSyncSystem>());
            group.AddSystem(container.Instantiate<MovementInputSystem>());
            group.AddSystem(container.Instantiate<MoveBufferWriteSystem>());
        }
    }
}