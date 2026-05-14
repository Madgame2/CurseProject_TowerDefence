using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class WindowInfo
{
    public string windowName;
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

    public bool ShowInStart = false;

    public List<string> canOpenWith;
    public List<string> cannotOpenWith;

    public bool selfCanvas = false;
    public bool withoutCanvas = false;
}