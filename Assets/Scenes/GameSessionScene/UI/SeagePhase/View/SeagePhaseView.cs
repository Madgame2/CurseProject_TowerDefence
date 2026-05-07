using Common.systems.UI;
using Common.systems.UI.View;
using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using Zenject;

public class SeagePhaseView : ViewBase<SeagePhaseViewModel>
{
    [SerializeField] private TMP_Text waveNum;

    [Inject] private UIManager _uiManager;

    [Header("Animation")]
    [SerializeField] private RectTransform window;
    [SerializeField] private CanvasGroup canvasGroup;

    [SerializeField] private float showDuration = 0.4f;
    [SerializeField] private float hideDuration = 0.4f;

    [SerializeField] private float visibleTime = 3f;

    [SerializeField] private float startYOffset = 300f;


    private Vector2 defaultPosition;

    private Tween currentTween;

    protected override void OnViewModelAssigned()
    {
        defaultPosition = window.anchoredPosition;

        // Начальное состояние
        window.anchoredPosition = defaultPosition + Vector2.up * startYOffset;
        canvasGroup.alpha = 0f;

        ViewModel.onWaveSeted += handleSetWave;
    }

    private async void handleSetWave(int wave)
    {
        waveNum.text = wave.ToString();

        await ShowWindow();

        await Awaitable.WaitForSecondsAsync(visibleTime);

        await HideWindow();
    }

    private async Awaitable ShowWindow()
    {
        currentTween?.Kill();

        Sequence sequence = DOTween.Sequence();

        sequence.Join(
            window.DOAnchorPos(defaultPosition, showDuration)
                .SetEase(Ease.OutBack)
        );

        sequence.Join(
            canvasGroup.DOFade(1f, showDuration)
        );

        currentTween = sequence;

        await sequence.AsyncWaitForCompletion();
    }

    private async Awaitable HideWindow()
    {
        currentTween?.Kill();

        Sequence sequence = DOTween.Sequence();

        sequence.Join(
            window.DOAnchorPos(
                    defaultPosition + Vector2.up * startYOffset,
                    hideDuration
                )
                .SetEase(Ease.InBack)
        );

        sequence.Join(
            canvasGroup.DOFade(0f, hideDuration)
        );

        currentTween = sequence;

        // Ждём окончания анимации
        await sequence.AsyncWaitForCompletion();

        _uiManager.Close("SeagePhaseView");
    }

    public override void Cleanup()
    {
        ViewModel.onWaveSeted -= handleSetWave;

        currentTween?.Kill();
    }
}
