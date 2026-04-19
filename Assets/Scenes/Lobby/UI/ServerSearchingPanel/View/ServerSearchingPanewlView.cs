using Common.systems.UI.View;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Scenes.Lobby
{
    public class SearchingPanelView : ViewBase<SearchingPanelViewModel>
    {
        [SerializeField] private TMP_Text _textField;
        [SerializeField] private string _initText;


        private string _baseText;

        private Coroutine _dotsCoroutine;
        protected override void OnViewModelAssigned()
        {
            _baseText = _initText;

            ViewModel.onTextChanged += onTextChangedHandler;

            _dotsCoroutine = StartCoroutine(DotsAnimation());
        }

        public override void Cleanup()
        {
            ViewModel.onTextChanged -= onTextChangedHandler;


            if (_dotsCoroutine != null)
            {
                StopCoroutine(_dotsCoroutine);
                _dotsCoroutine = null;
            }
        }

        public void onTextChangedHandler(string newText)
        {
            _baseText = newText;
        }

        private IEnumerator DotsAnimation()
        {
            int dotCount = 0;

            while (true)
            {
                dotCount = (dotCount + 1) % 4;
                // 0 → 1 → 2 → 3 → 0

                _textField.text = _baseText + new string('.', dotCount);

                yield return new WaitForSeconds(0.5f);
            }
        }
    }
}
