using Common.systems.Configs;
using UnityEngine;
using UnityEngine.Audio;
using Zenject;

public class AudioApplier : MonoBehaviour
{
    public AudioMixer mixer;
    [Inject]public ConfigSystem configSystem;


    void Start()
    {
        Apply(); 
    }


    public void Apply()
    {
        var cfg = configSystem.AudioProvider.config;

        mixer.SetFloat("MasterVolume", ToDb(cfg.TottalVolume));
        mixer.SetFloat("MusicVolume", ToDb(cfg.MusicVolume));
        mixer.SetFloat("AmbientVolume", ToDb(cfg.AmbientVolume));
        mixer.SetFloat("SFXVolume", ToDb(cfg.EffectsVolume));
    }

    private float ToDb(float value)
    {
        return Mathf.Log10(Mathf.Max(value, 0.0001f)) * 20;
    }
}
