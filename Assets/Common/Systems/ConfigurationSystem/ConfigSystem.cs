using Common.systems.Configs.Providers;
using UnityEngine;

namespace Common.systems.Configs
{
    using System;
    using System.IO;
    using UnityEngine;

    public class ConfigSystem
    {
        public AudioConfigProvider AudioProvider { get; private set; }
        public event Action hasChanges;

        private string _path;

        public ConfigSystem()
        {
            _path = Path.Combine(Application.persistentDataPath, "config.json");

            string json = LoadData();

            if (string.IsNullOrEmpty(json))
            {
                var defaultConfig = new AudioConfigs()
                {
                    TottalVolume = 1f,
                    MusicVolume = 1f,
                    AmbientVolume = 1f,
                    EffectsVolume = 1f
                };

                AudioProvider = new AudioConfigProvider(defaultConfig);
                SaveData();
            }
            else
            {
                var config = JsonUtility.FromJson<AudioConfigs>(json);
                AudioProvider = new AudioConfigProvider(config);
            }


            AudioProvider.onParamsChanges += HasChanges;
        }

        private void HasChanges()
        {
            hasChanges?.Invoke();
        }

        private string LoadData()
        {
            if (!File.Exists(_path))
                return null;

            return File.ReadAllText(_path);
        }

        public void SaveData()
        {
            var json = JsonUtility.ToJson(AudioProvider.config, true);
            File.WriteAllText(_path, json);
        }
    }
}
