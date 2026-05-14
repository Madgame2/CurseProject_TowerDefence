using Common.systems.UI.View;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndGameView : ViewBase<EndGameViewModel>
{
    [SerializeField] private TMP_Text waveNum;

    [SerializeField] private Button _toLobby;
    [SerializeField] private Button _leaveGame;
    protected override void OnViewModelAssigned()
    {
        ViewModel.onWaveSelected += handleWaveSelected;

        _leaveGame.onClick.AddListener(ViewModel.onExitGameHandler);
    }

    private void handleWaveSelected(int obj)
    {
        this.waveNum.text = obj.ToString();
    }

    public override void Cleanup()
    {
        ViewModel.onWaveSelected -= handleWaveSelected;

        _leaveGame.onClick.RemoveAllListeners();

    }
}
