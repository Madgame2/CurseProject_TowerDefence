using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class WindowInfo
{
    public string windowName;           
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
    public bool ShowInStart = false;
    public List<string> canOpenWith;    
    public List<string> cannotOpenWith; 
}