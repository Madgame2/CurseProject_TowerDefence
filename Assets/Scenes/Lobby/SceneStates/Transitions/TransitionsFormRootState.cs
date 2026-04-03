using Common.systems.SceneStates.Transitoins;
using UnityEngine;

public class TransitionsFormRootState : SceneStateTransitionRulesBase<MainLobbyState>
{
    public override void TransitList()
    {
        canTransitTo<SessionManagementLobbyState>();
        canTransitTo<SettingsLobbyState>();  
    }
}
