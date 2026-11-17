using System;
using Scenes.SessionRework.Scripts.ECS_World.Components.Geometry;
using Scenes.SessionRework.Scripts.ECS_World.Services.ProjectileSpawner.Interfaces;
using Scenes.SessionRework.Scripts.Services.Replication.Events.Spawn.Payload;
using Scenes.SessionRework.Scripts.Services.Replication.Models;
using Scenes.SessionRework.Scripts.Services.Replication.Router.Modules.Spawn.Modules.Base;
using UnityEngine;
using UnityEngine.ProBuilder;
using Zenject;

namespace Scenes.SessionRework.Scripts.Services.Replication.Router.Modules.Spawn.Modules
{
    public class ProjectileSpawnModule: ISpawnModule
    {
        public ObjectType ObjectType => ObjectType.Projectile;
        
        [Inject] private IProjectileSpawner _projectileSpawner;
        [Inject] private ProjectileView _projectileView;
        public void Spawn(uint objectId, SpawnPayload payload)
        {
            var projectilePayload =  payload.projectilePayload;

            try
            {
                _projectileSpawner.SpawnProjectile(objectId,projectilePayload.prefabId)
                    .With(new PositionComponent { Position = projectilePayload.Position })
                    .With(new RotationComponent { Rotation = projectilePayload.Rotation });
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
            
            
            
            //var projectile = Object.Instantiate(_projectileView, projectilePayload.Position, projectilePayload.Rotation);

            //tempPrimitive.transform.position = projectilePayload.Position;
            //tempPrimitive.transform.rotation = projectilePayload.Rotation;
        }
    }
}