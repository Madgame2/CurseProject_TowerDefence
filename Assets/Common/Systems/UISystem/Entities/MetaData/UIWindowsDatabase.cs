using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "UIWindowsDatabase", menuName = "UI/WindowsDatabase")]
public class UIWindowsDatabase : ScriptableObject
{
    public List<WindowInfo> windows;

    // Поиск окна
    public WindowInfo GetWindow(string name) =>
        windows.Find(w => w.windowName == name);

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (windows == null)
            return;

        foreach (var window in windows)
        {
            if (window != null)
            {
                window.UpdateType();
            }
        }

        EditorUtility.SetDirty(this);
    }
#endif
}