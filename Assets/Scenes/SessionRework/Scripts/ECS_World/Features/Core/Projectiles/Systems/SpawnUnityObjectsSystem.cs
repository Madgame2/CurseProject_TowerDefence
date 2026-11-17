using Scellecs.Morpeh;
using Scenes.SessionRework.Scripts.ECS_World.Components.Common;
using Scenes.SessionRework.Scripts.ECS_World.Components.Geometry;
using Scenes.SessionRework.Scripts.ECS_World.Features.Core.Projectiles.Components;
using Zenject;

namespace Scenes.SessionRework.Scripts.ECS_World.Features.Core.Projectiles.Systems
{
    public class SpawnUnityObjectsSystem: ISystem
    {
        public World World { get; set; }
        
        private Filter _filter;
        private Stash<UnityViewComponent> _unityViewStash;
        private Stash<ProjectilePrefabComponent> _projectilePrefabStash;
        private Stash<PositionComponent> _positionStash;
        private Stash<RotationComponent> _rotationStash;
        
        private ProjectileView _projectilePrefab;
        private IInstantiator _instantiator;
        
        public SpawnUnityObjectsSystem(World world, ProjectileView projectilePrefab, IInstantiator instantiator)
        {
            World = world;
            _projectilePrefab = projectilePrefab;
            _instantiator = instantiator;
        }

        public void OnAwake()
        {
            _filter = World.Filter
                .With<IDComponent>()
                .With<ProjectilePrefabComponent>()
                .Without<UnityViewComponent>()
                .Build();
            
            _projectilePrefabStash = World.GetStash<ProjectilePrefabComponent>();
            _unityViewStash = World.GetStash<UnityViewComponent>();
            _positionStash= World.GetStash<PositionComponent>();
            _rotationStash = World.GetStash<RotationComponent>();
        }
        
        public void OnUpdate(float deltaTime)
        {
            foreach (var projectileEntity in _filter)
            {
                var prefabComponent = _projectilePrefabStash.Get(projectileEntity);
                
                var spawnedProjectile = _instantiator.InstantiatePrefab(_projectilePrefab);
                _unityViewStash.Set(projectileEntity, new UnityViewComponent
                {
                    GameObject = spawnedProjectile.gameObject,
                });
                
                try
                {
                    ref var positionComponent = ref _positionStash.Get(projectileEntity);
                    spawnedProjectile.transform.position = positionComponent.Position;
                }
                catch
                {
                    // ignored
                }
                
                try
                {
                    ref var rotationComponent = ref _rotationStash.Get(projectileEntity);
                    spawnedProjectile.transform.rotation = rotationComponent.Rotation;
                }
                catch
                {
                    // ignored
                }
            }
        }
        
        public void Dispose()
        {
            
        }
    }
}