using Common.systems.UI.View;
using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Scenes.Lobby
{
    public class SearchingComplite: ViewBase<SearchingCompliteViewModle>
    {
        [SerializeField] private TMP_Text _textField;
        [SerializeField] private string _initText;

        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _moveDistance = 200f;
        [SerializeField] private float _duration = 0.5f;

        protected override void OnViewModelAssigned()
        {
            ViewModel.onTextChanged += onTextChangedHandler;

            var startPos = transform.localPosition;

            transform.localPosition = startPos + Vector3.up * _moveDistance;
            _canvasGroup.alpha = 0f;

            Sequence seq = DOTween.Sequence();

            seq.Join(transform.DOLocalMoveY(startPos.y, _duration).SetEase(Ease.OutCubic));
            seq.Join(_canvasGroup.DOFade(1f, _duration));
        }

        public override void Cleanup()
        {
            ViewModel.onTextChanged -= onTextChangedHandler;
        }

        public void onTextChangedHandler(string newText)
        {
            _textField.text = newText;
        }

    }
}
