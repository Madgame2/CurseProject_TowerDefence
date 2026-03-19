using UnityEngine;

namespace Common.systems.UI.View
{
    public interface IViewFor
    {
        void SetContext(object viewModel);
        void Cleanup();
    }
}
