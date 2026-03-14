using Common.Services.SceneServices.Params;
using Common.Services.SceneServices.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Common.Services.SceneServices
{
    public class SceneStateManager
    {
        private Dictionary<Type, SceneParamBase> _sceneParams = new();

        private SceneBase _currentScene;

        public Dictionary<Type, SceneParamBase> GetParams()
        {
            Dictionary<Type, SceneParamBase> parameters = _sceneParams;
            _sceneParams = new();

            return parameters;
        }

        public void loadScene<T>(params SceneParamBase[] parameters) where T : SceneBase
        {
            _sceneParams.Clear();
            foreach (var p in parameters)
            {
                _sceneParams[p.GetType()] = p;
            }

            if (_currentScene != null)
            {
                _currentScene.OnCleanup();
            }

            _currentScene = (SceneBase)Activator.CreateInstance(typeof(T));

            if (_currentScene != null)
            {
                _currentScene.OnPrepare();
                _currentScene.LoadMyScene();
            }
        }
    }
}
