using Scellecs.Morpeh;
using Scenes.SessionRework.Scripts.ECS_World.Components.Common;
using Scenes.SessionRework.Scripts.ECS_World.Components.Geometry;
using UnityEngine;

namespace Scenes.SessionRework.Scripts.ECS_World.Systems
{
    public class TransformSyncSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<PositionComponent> _positionStash;
        private Stash<UnityViewComponent> _unityViewStash;

        public TransformSyncSystem(World world)
        {
            World = world;
        }
        
        public void OnAwake()
        {
            _filter = World.Filter.With<PositionComponent>()
                .With<UnityViewComponent>()
                .Build();
            
            _positionStash = World.GetStash<PositionComponent>();
            _unityViewStash = World.GetStash<UnityViewComponent>();
        }
        
        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var pos = ref _positionStash.Get(entity);
                ref var view = ref _unityViewStash.Get(entity);
            
                if (view.Transform != null)
                {
                    view.Transform.position = Vector3.Lerp(view.Transform.position, pos.Position, deltaTime * 10f);
                }
            }
        }
        
        public void Dispose()
        {
            throw new System.NotImplementedException();
        }
    }
}