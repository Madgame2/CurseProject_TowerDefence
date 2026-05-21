using Common.systems.UI.View;
using System;
using TMPro;
using UnityEngine;

public class StatesViewer : ViewBase<SideStateViewModel>
{
    [SerializeField] private TMP_Text waveNum;
    [SerializeField] private TMP_Text buildCount;

    private int maxBuilded = 4;
    private int currentBuilded = 0;
    public override void Cleanup()
    {
        ViewModel.OnCurrentBuildChanged -= handleCurentBuildChaned;
        ViewModel.OnMaxBuildChanged -= handleMaxBuildChaned;

        ViewModel.OnWaveChanged -= handelWaveChanged;
    }

    protected override void OnViewModelAssigned()
    {
        ViewModel.OnCurrentBuildChanged += handleCurentBuildChaned;
        ViewModel.OnMaxBuildChanged += handleMaxBuildChaned;

        ViewModel.OnWaveChanged += handelWaveChanged;
    }

    private void handelWaveChanged(int obj)
    {
        waveNum.text = obj.ToString();
    }

    private void handleMaxBuildChaned(int obj)
    {
        maxBuilded = obj;
        RerenderBuildState();
    }

    private void handleCurentBuildChaned(int obj)
    {
        currentBuilded = obj;
        RerenderBuildState();
    }

    private void RerenderBuildState()
    {
        buildCount.text = $"{currentBuilded}/{maxBuilded}";
    }
}
