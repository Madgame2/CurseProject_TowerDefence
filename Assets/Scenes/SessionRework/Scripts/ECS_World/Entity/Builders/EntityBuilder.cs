using Scenes.SessionRework.Scripts.ECS_World.Entity.Builders.Interfaces;
using Scellecs.Morpeh;

namespace Scenes.SessionRework.Scripts.ECS_World.Entity.Builders
{
    public class EntityBuilder : IEntityBuilder
    {
        private readonly Scellecs.Morpeh.World _world;

        public EntityBuilder(Scellecs.Morpeh.World world)
        {
            _world = world;
        }

        public IEntityBuilder Create()
        {
            throw new System.NotImplementedException();
        }
    }
}