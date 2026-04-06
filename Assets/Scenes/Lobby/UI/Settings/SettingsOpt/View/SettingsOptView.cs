using Common.systems.SceneStates;
using Common.systems.UI.View;
using UnityEngine;
using UnityEngine.UI;


public class SettingsOptView : ViewBase<SettingsOptViewModel>
{
    [SerializeField] private ToggleVisualSettings visualSettings;

    [SerializeField] private Image GraphicsImage;
    [SerializeField] private Image AudioImage;
    [SerializeField] private Image ControlsImage;


    [SerializeField] private Toggle GraphicsOption;
    [SerializeField] private Toggle AudioSettings;
    [SerializeField] private Toggle ControlsSettings;

    [SerializeField] private Button ExitButton;

    private bool _isUpdating = false;

    protected override void OnViewModelAssigned()
    {
        ViewModel.ChangeButtonsAvailable += setButtonsActive;

        GraphicsOption.onValueChanged.AddListener(OnGraphics);
        AudioSettings.onValueChanged.AddListener(OnAudio);
        ControlsSettings.onValueChanged.AddListener(OnControls);

        ExitButton.onClick.AddListener(ViewModel.onBack);

        UpdateVisuals();
    }

    public override void Cleanup()
    {
        GraphicsOption.onValueChanged.RemoveAllListeners();
        AudioSettings.onValueChanged.RemoveAllListeners();
        ControlsSettings.onValueChanged.RemoveAllListeners();

        ExitButton.onClick.RemoveAllListeners();

        ViewModel.ChangeButtonsAvailable -= setButtonsActive;
    }

    public void OnGraphics(bool isOn)
    {
        if (!isOn) return;

        SelectOnly(GraphicsOption);
        UpdateVisuals();

        ViewModel.OnGraphics();
    }

    public void OnAudio(bool isOn)
    {
        if (!isOn) return;

        SelectOnly(AudioSettings);
        UpdateVisuals();

        ViewModel.OnAudio();
    }

    public void OnControls(bool isOn)
    {
        if (!isOn) return;

        SelectOnly(ControlsSettings);
        UpdateVisuals();

        ViewModel.OnControls();
    }

    private void SelectOnly(Toggle selected)
    {
        _isUpdating = true;

        GraphicsOption.SetIsOnWithoutNotify(GraphicsOption == selected);
        AudioSettings.SetIsOnWithoutNotify(AudioSettings == selected);
        ControlsSettings.SetIsOnWithoutNotify(ControlsSettings == selected);

        _isUpdating = false;
    }

    private void setButtonsActive(bool active)
    {
        SetToggleState(GraphicsOption, active);
        SetToggleState(AudioSettings, active);
        SetToggleState(ControlsSettings, active);
    }

    private void SetToggleState(Toggle toggle, bool active)
    {
        bool wasOn = toggle.isOn;

        toggle.interactable = active;

        // важно: без Notify, чтобы не вызвать OnValueChanged
        toggle.SetIsOnWithoutNotify(wasOn);
    }


    private void UpdateVisuals()
    {
        SetVisual(GraphicsOption, GraphicsImage);
        SetVisual(AudioSettings, AudioImage);
        SetVisual(ControlsSettings, ControlsImage);
    }

    private void SetVisual(Toggle toggle, Image image)
    {
        image.color = toggle.isOn
            ? visualSettings.activeColor
            : visualSettings.inactiveColor;
    }
}
