using Common.systems.GameStates;
using UnityEngine;
using Zenject;

public class DebugSceneInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        var gameStatemachine = Container.Resolve<GameStateMachine>();
        gameStatemachine.SetStartState<DebugState>();
    }
}
