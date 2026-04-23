using Common.systems.UI.View;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scenes.Session
{

    public class LoadingView : ViewBase<LoadingViewModel>
    {
        [SerializeField] private VignetteController vignetteController;
        [SerializeField] private Slider progressBar;
        [SerializeField] private TMP_Text statusText;
        protected override void OnViewModelAssigned()
        {
            vignetteController.FadeIn();

            ViewModel.onProgressBarUpdate += ProgressBarUpdateHandler;
            ViewModel.onStatusUpdated += StatusUpdatedHandle;
        }

        public override void Cleanup()
        {

            ViewModel.onProgressBarUpdate -= ProgressBarUpdateHandler;
            ViewModel.onStatusUpdated -= StatusUpdatedHandle;

            ViewModel.cleanUp();
        }

        private void StatusUpdatedHandle (string status)
        {
            statusText.text = status;
        }

        private void ProgressBarUpdateHandler(float newValue)
        {
            progressBar.DOValue(newValue, 0.5f).SetEase(Ease.OutCubic);
        }
    }
}
