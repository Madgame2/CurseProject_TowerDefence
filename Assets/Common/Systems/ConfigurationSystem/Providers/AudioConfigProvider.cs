using System;
using UnityEngine;

namespace Common.systems.Configs.Providers
{
    public class AudioConfigProvider
    {
        public AudioConfigs config { get; private set; }

        public event Action onParamsChanges;

        public AudioConfigProvider(AudioConfigs config)
        {
            this.config = config;
        }



        public void SetMasterVolume(float value)
        {
            config.TottalVolume = Mathf.Clamp01(value);
            onParamsChanges?.Invoke();
        }

        public void SetMusicVolume(float value)
        {
            config.MusicVolume = Mathf.Clamp01(value);
            onParamsChanges?.Invoke();
        }

        public void SetAmbientVolume(float value)
        {
            config.AmbientVolume = Mathf.Clamp01(value);
            onParamsChanges?.Invoke();
        }

        public void SetEffectsVolume(float value)
        {
            config.EffectsVolume = Mathf.Clamp01(value);
            onParamsChanges?.Invoke();
        }
    }
}
