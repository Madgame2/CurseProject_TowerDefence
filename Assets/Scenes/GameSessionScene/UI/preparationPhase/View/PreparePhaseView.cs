using UnityEngine;
using Common.systems.UI;
using Common.systems.UI.View;
using TMPro;
using System;
using Zenject;

public class PreparePhaseView : ViewBase<PreparationPhaseViewModel>
{
    [SerializeField] private TMP_Text _time;

    [Inject]private UIManager _uiManager;

    protected override void OnViewModelAssigned()
    {
        ViewModel.OnTimerChanged += handleChangedTimer;
        ViewModel.OnTimerFinished += handleFinishTiner;
    }

    private void handleChangedTimer(int seconds)
    {
        int minutes = seconds / 60;
        int sec = seconds % 60;

        _time.text = $"{minutes:00}:{sec:00}";
    }

    private void handleFinishTiner()
    {
        _uiManager.Close("PreparePhaseView");
    }

    public override void Cleanup()
    {
        ViewModel.OnTimerChanged -= handleChangedTimer;
        ViewModel.OnTimerFinished -= handleFinishTiner;
    }
}
