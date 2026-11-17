using Scellecs.Morpeh;
using Scenes.SessionRework.Scripts.ECS_World.Components.Common;
using Scenes.SessionRework.Scripts.ECS_World.Components.Geometry;
using Scenes.SessionRework.Scripts.ECS_World.Features.Core.Projectiles.Components;

namespace Scenes.SessionRework.Scripts.ECS_World.Services.ProjectileSpawner.Builder
{
    public struct ProjectileBuilder
    {
        private World _world;
        private Scellecs.Morpeh.Entity procesingEntity;
        
        public ProjectileBuilder(World world)
        {
            _world = world;
            procesingEntity = default;
        }

        public ProjectileBuilder With<T>(T component) where T: struct,IComponent
        {
            _world.GetStash<T>().Set(procesingEntity, component);
            
            return this;
        }

        public ProjectileBuilder Begin(uint objectId, uint prefabId)
        {
            procesingEntity = _world.CreateEntity();
            
            _world.GetStash<IDComponent>().Set(procesingEntity, new IDComponent{NetId = objectId});
            _world.GetStash<ProjectileTag>().Set(procesingEntity);
            _world.GetStash<ProjectilePrefabComponent>().Set(procesingEntity, new ProjectilePrefabComponent{PrefabID = prefabId});
            
            return this;
        }
    }
}