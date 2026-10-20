using Zenject;

namespace Scenes.SessionRework.Scripts.GameWorld.Core
{
    public class WorldHolder
    {
        public WorldContext WorldContainer { get; private set; }

        public void SetWorldContainer(WorldContext container)
        {
            WorldContainer = container;
        }
    }
    
    public sealed class WorldContext
    {
        public DiContainer DiContainer { get; }
        public Scellecs.Morpeh.World EcsWorld { get; }

        public WorldContext(
            DiContainer diContainer,
            Scellecs.Morpeh.World  ecsWorld)
        {
            DiContainer = diContainer;
            EcsWorld = ecsWorld;
        }
    }
}