using Common.Services.SceneServices.Params;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Common.Services.SceneServices
{
    public class SceneParamsReader
    {
        private Dictionary<Type, SceneParamBase> _parametrs = null;


        public int Count { get { return _parametrs.Count; } }

        [Inject]
        public void ReadParams(SceneStateManager sceneStaemannager)
        {
            _parametrs = sceneStaemannager.GetParams();
        }


        public SceneParamBase this[Type key]
        {
            get
            {
                return _parametrs.TryGetValue(key, out var value)
                    ? value
                    : null;
            }
        }

        public bool TryGetParam<T>(out T outParam) where T : SceneParamBase
        {
            outParam = null;

            if (!_parametrs.ContainsKey(typeof(T)))
                return false;


            var value = _parametrs[typeof(T)];

            if (value is T typedParam)
            {
                outParam = typedParam;
                return true;
            }

            return false;
        }
    }
}