using Common.systems.UI.PagesSystem;
using Common.systems.UI.View;
using UnityEngine;

public class SettingsImpView : ViewBase<SettingImpViewModel>
{
    [SerializeField] private PagesContainer _pages;

    protected override void OnViewModelAssigned()
    {
        ViewModel.Pages = _pages;
    }

    public override void Cleanup()
    {
        ViewModel.Pages = null;
    }
}
