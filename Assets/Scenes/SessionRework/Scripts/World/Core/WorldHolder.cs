using Zenject;

namespace Scenes.SessionRework.Scripts.World.Core
{
    public class WorldHolder
    {
        public DiContainer WorldContainer { get; private set; }

        public void SetWorldContainer(DiContainer container)
        {
            WorldContainer = container;
        }
    }
}