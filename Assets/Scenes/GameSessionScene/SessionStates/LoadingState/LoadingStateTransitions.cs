using Common.systems.SceneStates.Transitoins;


public class LoadingStateTransitions : SceneStateTransitionRulesBase<LoadingState>
{
    public override void TransitList()
    {
        canTransitTo<ProcessSessionState>();
    }
}
