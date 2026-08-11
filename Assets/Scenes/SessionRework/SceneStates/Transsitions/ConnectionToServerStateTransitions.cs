using Common.systems.SceneStates.Transitoins;
using Scenes.SessionRework.SceneStates.States;

namespace Scenes.SessionRework.SceneStates.Transsitions
{
    public class ConnectionToServerStateTransitions: SceneStateTransitionRulesBase<ConnectionToServerState>
    {
        public override void TransitList()
        {
            canTransitTo<SyncState>();
        }
    }
}