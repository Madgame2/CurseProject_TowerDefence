using Scenes.SessionRework.Scripts.World.Core.Data;
using Scenes.SessionRework.Scripts.World.Core.Orchestrator;
using Scenes.SessionRework.Scripts.World.Entities;
using Scenes.SessionRework.Scripts.World.Factories;
using Scenes.SessionRework.Scripts.World.Graph.BiomGraph.Interfaces;
using Scenes.SessionRework.Scripts.World.Graph.LandscapeGraph.Interfaces;
using Scenes.SessionRework.Scripts.World.Interfaces;
using Scenes.SessionRework.Scripts.World.Pool;
using Scenes.SessionRework.Scripts.World.Providers;
using Zenject;

namespace Installers
{
    public class WorldInstaller : Installer<WorldInitializationData, ChunkReader, WorldInstaller>
    {
        private readonly WorldInitializationData _initData;
        private readonly ChunkReader _chunkReaderPrefab;

        public WorldInstaller(WorldInitializationData initData, ChunkReader chunkReaderPrefab)
        {
            _initData = initData;
            _chunkReaderPrefab = chunkReaderPrefab;
        }
        
        public override void InstallBindings()
        {
            // 1. Биндим настройки и графы как инстансы
            Container.BindInstance(_initData.Settings).AsSingle();
            Container.BindInstance(_initData.LandscapeGraph).AsSingle();
            Container.BindInstance(_initData.BiomeGraph).AsSingle();
            Container.BindInstance(_chunkReaderPrefab).WhenInjectedInto<ChunkReaderPool>();

            // 2. Биндим фабрику и генератор (теперь они живут внутри мира)
            Container.BindInterfacesAndSelfTo<ChunkGeneratorFactory>().AsTransient();
            Container.Bind<IChunkGenerator>().To<ChunkGenerator>().AsTransient(); // или AsSingle, в зависимости от логики

            // 3. Остальной мир
            Container.BindInterfacesAndSelfTo<ChunkOrchestrator>().AsSingle();
            Container.BindInterfacesAndSelfTo<ChunkReaderPool>().AsSingle();
            Container.BindInterfacesAndSelfTo<ChunkCache>().AsSingle();
            Container.BindInterfacesAndSelfTo<WorldProvider>().AsSingle();
        }
    }
}