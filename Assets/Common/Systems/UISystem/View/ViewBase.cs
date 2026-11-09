using Common.systems.UI.View;
using UnityEngine;
using Zenject;

namespace Common.systems.UI.View
{
    public class ViewBase<TViewModel> : MonoBehaviour, IViewFor
    {
        protected TViewModel ViewModel { get; private set; }

        [Inject] private DiContainer _container;

        protected virtual void OnViewModelAssigned()
        {
            // Например, подписка на события ViewModel
        }

        public virtual void Cleanup()
        {
            // Отписка от событий
        }

        public object GetViewModel()
        {
            return ViewModel;
        }

        public void Initialize()
        {
            var vm = _container.Instantiate<TViewModel>();
            SetContext(vm);
        }

        public void SetContext(object viewModel)
        {
            ViewModel = (TViewModel)viewModel;
            OnViewModelAssigned();
        }
    }
}