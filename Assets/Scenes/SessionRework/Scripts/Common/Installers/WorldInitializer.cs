using Installers;
using Scellecs.Morpeh;
using Scenes.SessionRework.Scripts.ECS_World.Components.Common;
using Scenes.SessionRework.Scripts.ECS_World.Components.Geometry;
using Scenes.SessionRework.Scripts.ECS_World.Components.Players;
using Scenes.SessionRework.Scripts.ECS_World.Factories.Interfaces;
using Scenes.SessionRework.Scripts.ECS_World.Factories.Player.Model;
using Scenes.SessionRework.Scripts.ECS_World.Installers;
using Scenes.SessionRework.Scripts.EntryPoints;
using Scenes.SessionRework.Scripts.GameWorld.Core;
using Scenes.SessionRework.Scripts.GameWorld.Core.Data;
using Scenes.SessionRework.Scripts.GameWorld.Core.interfaces.BackendParams;
using Scenes.SessionRework.Scripts.GameWorld.Entities.Chunk;
using UnityEngine;
using Zenject;

namespace Scenes.SessionRework.Scripts.Common.Installers
{
    public class WorldInitializer:IWorldInitializer
    {
        [Inject] private readonly IGetBackendParam _getBackendParam;
        [Inject] private readonly ChunkReader _chunkReaderPrefab;
        [Inject] private readonly DiContainer _container;
        [Inject] private readonly WorldHolder _worldContainer;
        
        public void Initialize()
        {
            if (!ValidateWorldData())
            {
                Debug.LogError("Попытка инициализировать мир до получения всех данных!");
                return;
            }
            
            var ecsWorld = CreateEcsWorld();
            var diWorldContainer = CreateWorldContainer(ecsWorld);
            
            InstallWorlds(diWorldContainer, ecsWorld);
            
            Debug.Log("Контекст мира успешно создан и проинициализирован!");
        }
        
        private void InstallWorlds(DiContainer diWorldContainer, Scellecs.Morpeh.World ecsWorld)
        {
            EcsWorldInstaller.Install(
                ecsWorld,
                diWorldContainer);
            
            _worldContainer.SetWorldContainer(
                new WorldContext(
                    diWorldContainer,
                    ecsWorld));
        }
        
        private Scellecs.Morpeh.World CreateEcsWorld()
        {
            var ecsWorld = Scellecs.Morpeh.World.Create();
            ecsWorld.UpdateByUnity = true;
            
            return ecsWorld;
        }

        private DiContainer CreateWorldContainer(World ecsWorld)
        {
            var worldContainer = _container.CreateSubContainer();

            //ZenjectManagersInstaller.Install(worldContainer);

            var pendingData = new WorldInitializationData(
                _getBackendParam.ChunksSettings,
                _getBackendParam.LandscapeGraphRoot,
                _getBackendParam.BiomeGraphRoot,
                _getBackendParam.DecorationsGraphRoot,
                ecsWorld
            );

            DiWorldInstaller.Install(
                worldContainer,
                pendingData,
                _chunkReaderPrefab);

            var playersInitializer = worldContainer.Resolve<PlayersInitializer>();
            playersInitializer.Initialize();

            return worldContainer;
        }
        
        private bool ValidateWorldData()
        {
            return _getBackendParam.ChunksSettings != null &&
                   _getBackendParam.LandscapeGraphRoot != null &&
                   _getBackendParam.BiomeGraphRoot != null &&
                   _getBackendParam.DecorationsGraphRoot != null;
        }
    }
}