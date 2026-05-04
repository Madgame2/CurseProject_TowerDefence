using Common.systems.UI;
using UnityEngine;
using Zenject;

public class MenuButtonHandler : MonoBehaviour
{
    [Inject] private UIManager _uiManager;
    public void HandleMenuButton()
    {
        _uiManager.TryOpen("GameMenu");
    }
}
