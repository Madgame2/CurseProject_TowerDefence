using Scellecs.Morpeh;
using Scenes.SessionRework.Scripts.ECS_World.Systems.Network;
using Zenject;

namespace Scenes.SessionRework.Scripts.ECS_World.Features
{
    public static class NetworkFeature
    {
        public static void AddFeature(World world, SystemsGroup updateGroup, DiContainer container)
        {
            updateGroup.AddSystem(container.Instantiate<NetworkInputSendSystem>());
            updateGroup.AddSystem(container.Instantiate<NetworkPlayerPositionApplySystem>());
        }
    }
}