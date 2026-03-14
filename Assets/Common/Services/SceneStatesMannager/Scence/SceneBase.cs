using ModestTree;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Common.Services.SceneServices.Scenes
{
    public abstract class SceneBase
    {
        public abstract string SceneName { get; }

        public void LoadMyScene()
        {
            if (SceneName == null|| SceneName.IsEmpty())
            {
                Debug.LogError($"[BaseScene] Не удалось загрузить сцену: свойство SceneName пустое или null в {this.GetType().Name}");
                return;
            }
            try
            {
                SceneManager.LoadScene(SceneName);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[BaseScene] Ошибка при загрузке сцены '{SceneName}' в {this.GetType().Name}: {ex.Message}");
            }
        }

        public virtual void OnPrepare() { }
        public virtual void OnCleanup() { }
    }
}
