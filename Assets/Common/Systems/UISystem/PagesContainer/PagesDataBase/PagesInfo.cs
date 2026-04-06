using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Common.systems.UI.PagesSystem.DataBase.Configs
{
    [System.Serializable]
    public class PagesInfo
    {
        public string pageUri;
        public GameObject prefab;
        [SerializeField] private MonoScript viewModelScript;

        [NonSerialized]
        private Type _viewModelType;

        public Type ViewModelType
        {
            get
            {
                if (_viewModelType == null && viewModelScript != null)
                {
                    _viewModelType = viewModelScript.GetClass();
                }
                return _viewModelType;
            }
        }
    }
}
