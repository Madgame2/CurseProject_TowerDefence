using Common.systems.UI.View;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Common.systems.UI.Prefabs
{
    public class DangerActionView : ViewBase<DangerActionViewModel>
    {
        [SerializeField]private TMP_Text _tittle;
        [SerializeField]private TMP_Text _description;

        [SerializeField]private Button _yesButton;
        [SerializeField]private Button _noButton;


        protected override void OnViewModelAssigned()
        {
            _yesButton.onClick.AddListener(OnYes);
            _noButton.onClick.AddListener(OnNo);
        }

        public override void Cleanup()
        {
            _yesButton.onClick.RemoveListener(OnYes);
            _noButton.onClick.RemoveListener(OnNo);
        }


        private void OnYes()
        {
            ViewModel.SetResult(true);
        }

        private void OnNo()
        {
            ViewModel.SetResult(false);
        }
    }
}
