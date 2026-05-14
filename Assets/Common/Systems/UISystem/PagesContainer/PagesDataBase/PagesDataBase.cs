using Common.systems.UI.PagesSystem.DataBase.Configs;
using System.Collections.Generic;
using UnityEngine;

namespace Common.systems.UI.PagesSystem.DataBase
{
    [CreateAssetMenu(fileName = "UIWindowsDatabase", menuName = "UI/PagesDatabase")]
    public class PagesDataBase : ScriptableObject
    {
        public List<PagesInfo> pages;
        public PagesInfo GetWindow(string name) => pages.Find(w => w.pageUri == name);
        public PagesInfo GetWindow(int Id)
        {

            if (Id >= 0 && Id < pages.Count)
                return pages[Id];
            else
                return null; 
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            foreach (var page in pages)
            {
                if (page != null)
                {
                    page.UpdateType();
                }
            }

            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
    }
}
