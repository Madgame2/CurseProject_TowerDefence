using Common.systems.UI.View;
using UnityEngine;

namespace Common.systems.UI.View
{
    public class ViewBase<TViewModel> : MonoBehaviour, IViewFor
    {
        protected TViewModel ViewModel { get; private set; }


        protected virtual void OnViewModelAssigned()
        {
            // Например, подписка на события ViewModel
        }

        public virtual void Cleanup()
        {
            // Отписка от событий
        }

        public void SetContext(object viewModel)
        {
            ViewModel = (TViewModel)viewModel;
            OnViewModelAssigned();
        }
    }
}