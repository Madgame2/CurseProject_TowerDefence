using Common.systems.SceneStates.Transitoins;
using UnityEngine;

public class TransitionsFromConnecting : SceneStateTransitionRulesBase<ConnectingState>
{
    public override void TransitList()
    {
        canTransitTo<AuthState>();
    }
}
