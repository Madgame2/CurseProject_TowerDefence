using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Common.systems.UI.PagesSystem.DataBase.Configs
{
    [System.Serializable]
    public class PagesInfo
    {
        public string pageUri;
        public GameObject prefab;

#if UNITY_EDITOR
        [SerializeField]
        private MonoScript viewModelScript;
#endif

        [SerializeField]
        private string viewModelTypeName;

        [NonSerialized]
        private Type _viewModelType;

        public Type ViewModelType
        {
            get
            {
                if (_viewModelType == null)
                {
                    _viewModelType = Type.GetType(viewModelTypeName);
                }

                return _viewModelType;
            }
        }

#if UNITY_EDITOR
        public void UpdateType()
        {
            if (viewModelScript != null)
            {
                Type type = viewModelScript.GetClass();

                if (type != null)
                {
                    viewModelTypeName = type.AssemblyQualifiedName;
                }
            }
        }
#endif
    }
}