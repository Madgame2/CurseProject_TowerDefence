using Scellecs.Morpeh;

namespace Scenes.SessionRework.Scripts.ECS_World.Systems.Input
{
    public class MakeSnapeshotSystem : IFixedSystem
    {
        public World World { get; set; }

        public MakeSnapeshotSystem(World world)
        {
            World = world;
        }

        public void OnAwake()
        {
            
        }

        public void OnUpdate(float deltaTime)
        {
            
        }

        public void Dispose()
        {
            
        }
    }
}