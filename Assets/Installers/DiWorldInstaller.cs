using Scenes.SessionRework.Scripts.ECS_World.Factories.Interfaces;
using Scenes.SessionRework.Scripts.ECS_World.Factories.Player;
using Scenes.SessionRework.Scripts.EntryPoints;
using Scenes.SessionRework.Scripts.GameWorld.Core.Data;
using Scenes.SessionRework.Scripts.GameWorld.Core.Orchestrator;
using Scenes.SessionRework.Scripts.GameWorld.Entities;
using Scenes.SessionRework.Scripts.GameWorld.Entities.Chunk;
using Scenes.SessionRework.Scripts.GameWorld.Factories;
using Scenes.SessionRework.Scripts.GameWorld.Graph.BiomGraph.Interfaces;
using Scenes.SessionRework.Scripts.GameWorld.Graph.LandscapeGraph.Interfaces;
using Scenes.SessionRework.Scripts.GameWorld.Interfaces;
using Scenes.SessionRework.Scripts.GameWorld.Pool;
using Scenes.SessionRework.Scripts.GameWorld.Providers;
using Scenes.SessionRework.Scripts.Player.View;
using Zenject;

namespace Installers
{
    public class DiWorldInstaller : Installer<WorldInitializationData, ChunkReader, DiWorldInstaller>
    {
        private readonly WorldInitializationData _initData;
        private readonly ChunkReader _chunkReaderPrefab;

        public DiWorldInstaller(WorldInitializationData initData, ChunkReader chunkReaderPrefab)
        {
            _initData = initData;
            _chunkReaderPrefab = chunkReaderPrefab;
        }
        
        public override void InstallBindings()
        {
            // 1. Биндим настройки и графы как инстансы
            Container.BindInstance(_initData.EcsWorld);
            Container.BindInstance(_initData.Settings).AsSingle();
            Container.BindInstance(_initData.LandscapeGraph).AsSingle();
            Container.BindInstance(_initData.BiomeGraph).AsSingle();
            Container.BindInstance(_initData.DecorationsGraph).AsSingle();
            Container.BindInstance(_chunkReaderPrefab).WhenInjectedInto<ChunkReaderPool>();

            // 2. Биндим фабрику и генератор (теперь они живут внутри мира)
            Container.BindInterfacesAndSelfTo<ChunkGeneratorFactory>().AsTransient();
            Container.Bind<IChunkGenerator>().To<ChunkGenerator>().AsTransient(); // или AsSingle, в зависимости от логики

            // 3. Остальной мир
            Container.BindInterfacesAndSelfTo<ChunkOrchestrator>().AsSingle();
            Container.BindInterfacesAndSelfTo<ChunkReaderPool>().AsSingle();
            Container.BindInterfacesAndSelfTo<ChunkCache>().AsSingle();
            Container.BindInterfacesAndSelfTo<WorldProvider>().AsSingle();
            
            Container.Bind<IPlayerFactory>().To<PlayerFactory>().AsSingle();


            Container.Bind<PlayersInitializer>().AsSingle();
        }
    }
}