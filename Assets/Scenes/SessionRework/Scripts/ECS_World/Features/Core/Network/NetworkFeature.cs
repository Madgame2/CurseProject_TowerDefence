using Scellecs.Morpeh;
using Scenes.SessionRework.Scripts.ECS_World.Components.Addition;
using Scenes.SessionRework.Scripts.ECS_World.Components.Movement;
using Scenes.SessionRework.Scripts.ECS_World.Features.Components;
using Scenes.SessionRework.Scripts.ECS_World.Features.Systems;
using Scenes.SessionRework.Scripts.ECS_World.Systems.Network;
using Zenject;

namespace Scenes.SessionRework.Scripts.ECS_World.Features
{
    public static class NetworkFeature
    {
        public static void AddFeature(World world, SystemsGroup updateGroup, DiContainer container)
        {
            CreateEnity(world, container);
            
            updateGroup.AddSystem(container.Instantiate<WritePlayerStateToHistorySystem>());
            //updateGroup.AddSystem(container.Instantiate<ResetCurrentSnapshotSystem>());
            
            updateGroup.AddSystem(container.Instantiate<NetworkStateSendSystem>());
            updateGroup.AddSystem(container.Instantiate<NetworkPlayersStateApplySystem>());
            updateGroup.AddSystem(container.Instantiate<NetworkReplicationSystem>());
        }

        private static void CreateEnity(World world, DiContainer container)
        {
            var playerStateEntity = world.CreateEntity();

            var stateSnapshotStash = world.GetStash<CurrentStateSnapshotComponent>();
            var stateHistoryStash =  world.GetStash<PlayerStateHistoryComponent>();
            
            stateSnapshotStash.Set(playerStateEntity);
            stateHistoryStash.Set(playerStateEntity, new PlayerStateHistoryComponent
            {
                CurrentIndex = 0,
                Buffer = new PlayerStateSnapshot[15]
            });
        }
    }
}