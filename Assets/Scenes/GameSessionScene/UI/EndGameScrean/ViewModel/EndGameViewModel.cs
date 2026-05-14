using Common.systems.GameStates;
using System;
using UnityEngine;
using Zenject;

public class EndGameViewModel
{
    [Inject] private GameStateMachine gameStateMachine;


    public Action<int> onWaveSelected;

    public void SetWaveNum(int waveNum)
    {
        onWaveSelected?.Invoke(waveNum);
    }


    public void onExitGameHandler()
    {
        this.gameStateMachine.tryMoveToState(typeof(ExitState));
    }
}
