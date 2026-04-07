using Common.systems.Configs;
using System;
using UnityEngine;

public class AudioPageViewModel
{
    private ConfigSystem _configSystem;

    public AudioPageViewModel(ConfigSystem configSystem)
    {
        _configSystem = configSystem;
    }

    internal void SetEffectsVolume(float value)
    {
        _configSystem.AudioProvider.SetEffectsVolume(value);
    }

    internal void SetEnvironmentVolume(float value)
    {
        _configSystem.AudioProvider.SetAmbientVolume(value);
    }

    internal void SetMusicVolume(float value)
    {
        _configSystem.AudioProvider.SetMusicVolume(value);
    }

    internal void SetTotalVolume(float value)
    {
        _configSystem.AudioProvider.SetMasterVolume(value);
    }


    public float TotalVolume => _configSystem.AudioProvider.config.TottalVolume;
    public float MusicVolume => _configSystem.AudioProvider.config.MusicVolume;
    public float EffectsVolume => _configSystem.AudioProvider.config.EffectsVolume;
    public float EnvironmentVolume => _configSystem.AudioProvider.config.AmbientVolume;
}
