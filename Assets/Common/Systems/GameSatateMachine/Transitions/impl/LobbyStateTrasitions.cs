using Common.systems.GameStates.States;
using Common.systems.GameStates.Transitions;
using UnityEngine;

public class LobbyStateTrasitions : TransitionRulesBase<LobbyState>
{
    public override void TransistionList()
    {
        CanTrasitTo<ConnectToServerState>();
        CanTrasitTo<ExitState>();
        CanTrasitTo<GameSessionState>();
    }
}
