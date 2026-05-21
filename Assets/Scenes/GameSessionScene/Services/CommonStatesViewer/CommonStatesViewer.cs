using Common.systems.UI;
using System;
using UnityEngine;
using Zenject;

public class CommonStatesViewer : IInitializable, IDisposable
{
    [Inject] private UIManager _uiManager;


    private SideStateViewModel _sideStateViewModel;
    public void Dispose()
    {
        _uiManager.Close("SideGameInfo");
        _sideStateViewModel = null;
    }

    public void Initialize()
    {
        _uiManager.TryOpen("SideGameInfo",out object vm_obj);
        if(vm_obj is SideStateViewModel viewModel)
        {
            _sideStateViewModel = viewModel;
        }
    }

    internal void buildStates(BuildSystemInfo buidlSystem)
    {
        _sideStateViewModel.SetBuildCount(buidlSystem.currentBuilded, buidlSystem.max_Buildings);
    }

    internal void SetWave(int? wave)
    {
        _sideStateViewModel.SetWave(wave);
    }
}
