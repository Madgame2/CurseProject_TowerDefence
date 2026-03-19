using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UIWindowsDatabase", menuName = "UI/WindowsDatabase")]
public class UIWindowsDatabase : ScriptableObject
{
    public List<WindowInfo> windows;

    // Можно добавить метод для поиска окна по имени
    public WindowInfo GetWindow(string name) => windows.Find(w => w.windowName == name);
}
