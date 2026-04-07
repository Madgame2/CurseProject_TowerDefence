using Common.systems.UI.View;
using UnityEngine;
using UnityEngine.UI;

public class AudioPageView: ViewBase<AudioPageViewModel>
{
    [SerializeField] private Slider _totalVolume;
    [SerializeField] private Slider _musicVolume;
    [SerializeField] private Slider _effectsVolume;
    [SerializeField] private Slider _environmentVolume;

    protected override void OnViewModelAssigned()
    {
        _totalVolume.value = ViewModel.TotalVolume;
        _musicVolume.value = ViewModel.MusicVolume;
        _effectsVolume.value = ViewModel.EffectsVolume;
        _environmentVolume.value = ViewModel.EnvironmentVolume;

        _totalVolume.onValueChanged.AddListener(OnTotalVolumeChanged);
        _musicVolume.onValueChanged.AddListener(OnMusicVolumeChanged);
        _effectsVolume.onValueChanged.AddListener(OnEffectsVolumeChanged);
        _environmentVolume.onValueChanged.AddListener(OnEnvironmentVolumeChanged);
    }

    public override void Cleanup()
    {
        _totalVolume.onValueChanged.RemoveListener(OnTotalVolumeChanged);
        _musicVolume.onValueChanged.RemoveListener(OnMusicVolumeChanged);
        _effectsVolume.onValueChanged.RemoveListener(OnEffectsVolumeChanged);
        _environmentVolume.onValueChanged.RemoveListener(OnEnvironmentVolumeChanged);
    }


    private void OnTotalVolumeChanged(float value)
    {
        ViewModel.SetTotalVolume(value);
    }

    private void OnMusicVolumeChanged(float value)
    {
        ViewModel.SetMusicVolume(value);
    }

    private void OnEffectsVolumeChanged(float value)
    {
        ViewModel.SetEffectsVolume(value);
    }

    private void OnEnvironmentVolumeChanged(float value)
    {
        ViewModel.SetEnvironmentVolume(value);
    }
}
