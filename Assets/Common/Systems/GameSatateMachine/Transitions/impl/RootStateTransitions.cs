using Common.systems.GameStates.States;
using Common.systems.GameStates.Transitions;
using UnityEngine;

public class RootStateTransitions : TransitionRulesBase<ConnectToServerState>
{
    public override void TransistionList()
    {
        CanTrasitTo<ExitState>();
        CanTrasitTo<LobbyState>();
    }
}
