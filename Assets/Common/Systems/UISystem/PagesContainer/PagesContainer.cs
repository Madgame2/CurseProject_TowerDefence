using Common.systems.UI.PagesSystem.DataBase;
using Common.systems.UI.PagesSystem.DataBase.Configs;
using Common.systems.UI.View;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;
using System.Collections.Generic;

namespace Common.systems.UI.PagesSystem
{
    public class PagesContainer : MonoBehaviour
    {
        [SerializeField] private PagesDataBase _pagesDatabase;
        [SerializeField] private Transform _canvas;

        [Inject]private DiContainer container;

        private GameObject _currentPage;

        private readonly Dictionary<PagesInfo, GameObject> _pagesBuffer = new();

        public AdvancedOptions OpenPageID(int PageID)
        {
            if(_currentPage != null)
            {
                Destroy(_currentPage);
            }

            var config = _pagesDatabase.GetWindow(PageID);
            GameObject page = CreatePage(config);
            _currentPage = page;
            _currentPage.transform.SetParent(_canvas, false);

            return new AdvancedOptions(this, config, page);
        }

        public AdvancedOptions OpenPageByName(string Uri)
        {
            if (_currentPage != null)
            {
                if (_pagesBuffer.ContainsValue(_currentPage))
                {
                    _currentPage.SetActive(false);
                }
                else { 
                    Destroy(_currentPage);
                }
            }

            var config = _pagesDatabase.GetWindow(Uri);
            GameObject page = CreatePage(config);
            _currentPage = page;
            _currentPage.transform.SetParent(_canvas, false);

            return new AdvancedOptions(this, config, page);
        }

        private GameObject CreatePage(PagesInfo pageInfo)
        {
            if (_pagesBuffer.ContainsKey(pageInfo))
            {
                GameObject page = _pagesBuffer[pageInfo];
                page.SetActive(true);

                return page;
            }

            var go = container.InstantiatePrefab(pageInfo.prefab);

            var vm = container.Instantiate(pageInfo.ViewModelType);


            var view = go.GetComponent<IViewFor>();
            view.SetContext(vm);

            return go;
        }


        public class AdvancedOptions
        {
            private PagesContainer _container;
            private PagesInfo _view;
            private GameObject _go;

            public AdvancedOptions(PagesContainer container, PagesInfo currentPage, GameObject go)
            {
                _container = container; 
                _view = currentPage;
                _go = go;
            }

            public AdvancedOptions DontUnload()
            {
                if(!_container._pagesBuffer.ContainsKey(_view))
                {
                    _container._pagesBuffer.Add(_view, _go);
                }

                return this;
            }
        }
    }
}
