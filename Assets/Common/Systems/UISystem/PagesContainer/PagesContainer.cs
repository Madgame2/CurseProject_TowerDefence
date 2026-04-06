using Common.systems.UI.PagesSystem.DataBase;
using Common.systems.UI.PagesSystem.DataBase.Configs;
using Common.systems.UI.View;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;
using static UnityEditor.Profiling.HierarchyFrameDataView;

namespace Common.systems.UI.PagesSystem
{
    public class PagesContainer : MonoBehaviour
    {
        [SerializeField] private PagesDataBase _pagesDatabase;
        [SerializeField] private Transform _canvas;

        [Inject]private DiContainer container;

        private GameObject _currentPage;

        public void OpenPageID(int PageID)
        {
            if(_currentPage != null)
            {
                Destroy(_currentPage);
            }

            var config = _pagesDatabase.GetWindow(PageID);
            _currentPage = CreatePage(config);
            _currentPage.transform.SetParent(_canvas.transform);
        }

        public void OpenPageByName(string Uri)
        {
            if (_currentPage != null)
            {
                Destroy(_currentPage);
            }

            var config = _pagesDatabase.GetWindow(Uri);
            _currentPage = CreatePage(config);
            _currentPage.transform.SetParent(_canvas, false);
        }

        private GameObject CreatePage(PagesInfo pageInfo)
        {
            var go = container.InstantiatePrefab(pageInfo.prefab);


            var vm = container.Instantiate(pageInfo.ViewModelType);


            var view = go.GetComponent<IViewFor>();
            view.SetContext(vm);

            return go;
        }
    }
}
