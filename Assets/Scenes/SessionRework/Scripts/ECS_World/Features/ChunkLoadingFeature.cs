using Scellecs.Morpeh;
using Scenes.SessionRework.Scripts.ECS_World.Components.World;
using Scenes.SessionRework.Scripts.ECS_World.Systems.ChunkLogicSystems;
using Zenject;

namespace Scenes.SessionRework.Scripts.ECS_World.Features
{
    public static class ChunkLoadingFeature
    {
        public static void AddFeature(World world, SystemsGroup group, DiContainer container)
        {
            var chunkStorage =  world.CreateEntity();

            var chunkStorageStash = world.GetStash<ChunkStorageComponent>();
            chunkStorageStash.Set(chunkStorage, new ChunkStorageComponent{ChunkStorage = new()});

            group.AddSystem(container.Instantiate<PickChunkSystem>());
            group.AddSystem(container.Instantiate<ReturnChunkSystem>());
        }
    }
}