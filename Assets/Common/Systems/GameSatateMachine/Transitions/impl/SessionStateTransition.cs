using Common.systems.GameStates.States;
using Common.systems.GameStates.Transitions;
using UnityEngine;

public class SessionStateTransition : TransitionRulesBase<GameSessionState>
{
    public override void TransistionList()
    {
        CanTrasitTo<ExitState>();
        CanTrasitTo<ConnectToServerState>();
        CanTrasitTo<LobbyState>();
    }
}
