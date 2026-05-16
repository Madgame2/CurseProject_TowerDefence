using ModestTree;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Common.Services.SceneServices.Scenes
{
    public abstract class SceneBase
    {
        public abstract string SceneName { get; }
        private bool _isLoading;

        public void LoadMyScene()
        {
            if (_isLoading)
            {
                Debug.LogWarning("Scene already loading, ignored");
                return;
            }

            if (SceneName == null|| SceneName.IsEmpty())
            {
                Debug.LogError($"[BaseScene] Не удалось загрузить сцену: свойство SceneName пустое или null в {this.GetType().Name}");
                return;
            }
            try
            {
                if (SceneManager.GetActiveScene().name == SceneName) return;

                SceneManager.LoadScene(SceneName, LoadSceneMode.Single);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[BaseScene] Ошибка при загрузке сцены '{SceneName}' в {this.GetType().Name}: {ex.Message}");
            }
        }

        public virtual async Task LoadMySceneAsync()
        {
            if (_isLoading)
            {
                Debug.LogWarning("Scene already loading, ignored");
                return;
            }

            var operation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(SceneName, LoadSceneMode.Single);

            while (!operation.isDone)
            {
                await Task.Yield(); // даёт кадру отрисоваться
            }
        }

        public virtual void OnPrepare() { }
        public virtual void OnCleanup() { }
    }
}
