using Common.systems.UI.Prefabs;
using Common.systems.UI.View;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor.PackageManager;
using UnityEngine;
using Zenject;


namespace Common.systems.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private WindowInfo _dangerActionQuestionWindow;
        [SerializeField] private GameObject _messageWindow;

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

        public async Task<bool> QuestionWindow(string title, string description, DialogType type)
        {
            bool reult = false;
            switch (type) {
                case DialogType.Danger:
                    DangerActionViewModel vm = (DangerActionViewModel)openDangerQuestion();

                    reult = await vm.GetResult();
                    closeDangerQuestion();
                    break;
            }

            return reult;
        }

        private object openDangerQuestion()
        {

            var go = container.InstantiatePrefab(_dangerActionQuestionWindow.prefab, canvas.transform);


            var vm = container.Instantiate(_dangerActionQuestionWindow.ViewModelType);


            var view = go.GetComponent<IViewFor>();
            view.SetContext(vm);

            openedWindows[_dangerActionQuestionWindow] = view;
            return vm;
        }

        private bool closeDangerQuestion()
        {

            if (!openedWindows.TryGetValue(_dangerActionQuestionWindow, out var view) || view == null)
                return false; // окно не открыто

            view.Cleanup();

            Destroy((view as MonoBehaviour).gameObject);

            openedWindows.Remove(_dangerActionQuestionWindow);

            return true;
        }

        public AdvancedOptions TryOpen(string windowName)
        {
            return TryOpen(windowName, out _);
        }

        public AdvancedOptions TryOpen(string windowName, out object ViewModel)
        {
            ViewModel = null;
            var winInfo = windowsDatabase.GetWindow(windowName);
            if (openedWindows.ContainsKey(winInfo)) return null;

            Transform root = winInfo.selfCanvas ? null : canvas?.transform;
            var go = container.InstantiatePrefab(winInfo.prefab, root);


            var vm = container.Instantiate(winInfo.ViewModelType);


            var view = go.GetComponent<IViewFor>();
            view.SetContext(vm);


            openedWindows[winInfo] = view;
            ViewModel = vm;

            AdvancedOptions options = new AdvancedOptions(this);
            return options;
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

        public void showInformWindow(string Tittle, string description)
        {
            var obj = Instantiate(_messageWindow, canvas.transform);
            obj.TryGetComponent<MessageWindowView>(out MessageWindowView view);

            if (view != null)
            {
                view.Init(Tittle, description);
            }
        }

        public void Hide(string uri)
        {
            var winInfo = windowsDatabase.GetWindow(uri);

            if (!openedWindows.TryGetValue(winInfo, out var view) || view == null)
                return; // окно не открыто

            if(view is MonoBehaviour viewMono)
            {
                viewMono.gameObject.SetActive(false);
            }
        }

        public void Show(string uri)
        {
            var winInfo = windowsDatabase.GetWindow(uri);

            if (!openedWindows.TryGetValue(winInfo, out var view) || view == null)
                return; // окно не открыто

            if (view is MonoBehaviour viewMono)
            {
                viewMono.gameObject.SetActive(true);
            }
        }

        public class AdvancedOptions
        {
            private readonly UIManager _uiManager;

            public AdvancedOptions(UIManager uiManager)
            {
                _uiManager = uiManager;
            }

            public void Hide(string pageName)
            {
                _uiManager.Hide(pageName);
            }
        }
    }
}