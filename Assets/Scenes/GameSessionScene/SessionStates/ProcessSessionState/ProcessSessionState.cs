using Common.systems.SceneStates.States;
using Common.systems.SceneStates.States.Attributes;
using Common.systems.UI;
using UnityEngine;
using Zenject;

[LinkToScene(typeof(GameSessionScene))]
public class ProcessSessionState : BaseState
{
    [Inject] private NetDispatcher _netDispatcher;
    [Inject] private UIManager _uiManager;

    public override void EnterToState()
    {
        _uiManager.TryOpen("GrossCard");
        _uiManager.TryOpen("TeslaTowerCard");
        _uiManager.TryOpen("KnightCampKnight");

        _netDispatcher.Init();
    }
}
