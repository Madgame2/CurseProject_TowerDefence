using UnityEngine;
using Common.systems.UI;
using Common.systems.UI.View;
using Common.systems.UI.PagesSystem;

public class LobbyView : ViewBase<LobbyViewModel>
{
    [SerializeField] private PagesContainer _pagesContainer;


    protected override void OnViewModelAssigned()
    {
        ViewModel.InitViewModel(_pagesContainer);
    }

    public override void Cleanup()
    {
        ViewModel.cleanUp();
    }
}
