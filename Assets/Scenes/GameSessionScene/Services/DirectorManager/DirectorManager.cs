using Common.systems.UI;
using System;
using UnityEngine;
using Zenject;

public class DirectorManager
{

    [Inject] private UIManager _uiManager;

    internal void HandleUpdate(DirectorEvent director)
    {
        switch (director.matchPahase)
        {
            case MatchPhase.PREPARATION:
                {
                    var data = director.data.ToObject<PreparationPhaseDTO>();
                    if(data != null)
                    {
                        showPreparationUI(data.countdown);
                    }

                    break;
                }
            case MatchPhase.WAVE:
                {
                    var data = director.data.ToObject<WavePhaseDTO>();
                    if (data != null)
                        showSeagePhaseUi(data.wave);
                    break;
                }
        }
    }

    private void showSeagePhaseUi(int waveNum)
    {
        _uiManager.TryOpen("SeagePhaseView", out object vm);
        if(vm is SeagePhaseViewModel viewModel)
        {
            viewModel.SetWave(waveNum);
        }
    }

    private void showPreparationUI(int countDown)
    {
        _uiManager.TryOpen("PreparePhaseView", out object vm);
        if(vm is PreparationPhaseViewModel ViewModel)
        {
            ViewModel.SetTimer(countDown);
        }
    }
}
