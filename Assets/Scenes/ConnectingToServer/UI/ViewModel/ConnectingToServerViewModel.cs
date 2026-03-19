using Common.systems.GameStates;
using Common.systems.UI;
using UnityEngine;

public class ConnectingToServerViewModel
{
    private readonly GameStateMachine gameStates;
    private readonly UIManager uIManager;
    public ConnectingToServerViewModel(GameStateMachine statesmachine, UIManager ui)
    {
        this.gameStates = statesmachine;
        this.uIManager = ui;
    }

    public void exitFormGame()
    {
        gameStates.tryMoveToState(typeof(ExitState));
    }

    public void swapToCreateAccountView()
    {
        uIManager.Close("LogIn");
        uIManager.TryOpen("CreateAccount");
    }
}
