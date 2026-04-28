using Common.systems.SceneStates.States;
using Common.systems.SceneStates.States.Attributes;
using UnityEngine;
using Zenject;

[LinkToScene(typeof(GameSessionScene))]
public class ProcessSessionState : BaseState
{
    [Inject] private NetDispatcher _netDispatcher;

    public override void EnterToState()
    {
        _netDispatcher.Init();
    }
}
