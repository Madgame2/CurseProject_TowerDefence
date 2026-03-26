using Common.systems.SceneStates.Transitoins;
using UnityEngine;

public class TransitionsFromRoot : SceneStateTransitionRulesBase<AuthState>
{
    public override void TransitList()
    {
        canTransitTo<ConnectingState>();
    }
}
