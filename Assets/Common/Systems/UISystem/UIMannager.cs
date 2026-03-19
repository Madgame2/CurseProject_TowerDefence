using Common.systems.UI.View;
using System.Collections.Generic;
using UnityEngine;
using Zenject;


namespace Common.systems.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private UIWindowsDatabase windowsDatabase;
        [SerializeField] private Canvas canvas;

        [Inject]private readonly DiContainer container;
        private readonly Dictionary<WindowInfo, IViewFor> openedWindows = new();


        public void Start()
        {
            foreach (var window in windowsDatabase.windows)
            {
                if (window.ShowInStart)
                {
                    TryOpen(window.windowName);
                }
            }
        }

        public void TryOpen(string windowName)
        {
            var winInfo = windowsDatabase.GetWindow(windowName);
            if (openedWindows.ContainsKey(winInfo)) return;


            var go = container.InstantiatePrefab(winInfo.prefab, canvas.transform);


            var vm = container.Instantiate(winInfo.ViewModelType);


            var view = go.GetComponent<IViewFor>();
            view.SetContext(vm);


            openedWindows[winInfo] = view; 
        }

        public bool Close(string windowName)
        {
            var winInfo = windowsDatabase.GetWindow(windowName);

            if (!openedWindows.TryGetValue(winInfo, out var view) || view == null)
                return false; // окно не открыто

            view.Cleanup();

            Destroy((view as MonoBehaviour).gameObject);

            openedWindows.Remove(winInfo);

            return true;
        }

    }
}