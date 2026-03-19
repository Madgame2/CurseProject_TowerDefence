using Common.systems.GameStates;
using Common.systems.UI;
using UnityEngine;

public class CreateAccountViewModel
{
    private readonly GameStateMachine gameStateMachine;
    private readonly UIManager uIManager;

    public CreateAccountViewModel(GameStateMachine gameStateMachine, UIManager uIManager)
    {
        this.gameStateMachine = gameStateMachine;
        this.uIManager = uIManager;
    }

    public void swapToLogIN()
    {
        uIManager.Close("CreateAccount");
        uIManager.TryOpen("LogIn");
    }

    public void CloseGame()
    {
        gameStateMachine.tryMoveToState(typeof(ExitState));
    }
}
