using Common.systems.UI.View;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PopUppanelView : ViewBase<PopUpPanelViewModel>
{
    [SerializeField] private Button onProfileEdit;
    [SerializeField] private Button onLogOut;
    [SerializeField] private RectTransform panelArea;

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();

            bool isInside = RectTransformUtility.RectangleContainsScreenPoint(
                panelArea,
                mousePosition
            );

            if (!isInside)
            {
                OnClickOutside();
            }
        }
    }

    private void OnClickOutside()
    {
        ViewModel.closePanel();
    }

    protected override void OnViewModelAssigned()
    {
        onLogOut.onClick.AddListener(ViewModel.onLogOuthandler);
        onProfileEdit.onClick.AddListener(ViewModel.EditProfile);
    }

    public override void Cleanup()
    {
        onLogOut.onClick.RemoveAllListeners();
        onProfileEdit.onClick.RemoveAllListeners();
    }
}