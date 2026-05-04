using Common.systems.UI.View;
using DG.Tweening;
using System;
using UnityEngine;

public class CardView : ViewBase<CardViewModel>
{
    [SerializeField] private RectTransform _rect;
    [SerializeField] private CanvasGroup _canvasGroup;

    [SerializeField] private float hoverLift = 20f;
    [SerializeField] private float selectLift = 60f;

    [SerializeField] private BuildObjectsEnum _buildObject;

    private Vector3 _startPos;

    private void Awake()
    {
        _startPos = _rect.localPosition;
    }

    // ===== ВЫЗЫВАЕТСЯ ИЗ DRAG HANDLER =====
    public void UpdateGhost(Vector3 worldPosition)
    {
        ViewModel.UpdateGhost(worldPosition, _buildObject);
    }


    public void OnHoverEnter()
    {
        Lift(hoverLift);
    }

    public void OnHoverExit()
    {
        ResetPosition();
    }

    public void OnSelect()
    {
        Lift(selectLift);
        _rect.SetAsLastSibling();
    }

    public void OnDragInside()
    {
        Show();
    }

    public void OnDragOutside()
    {
        Hide();
    }

    public void OnReleasedInside()
    {
        Show();
    }

    public void OnReleasedOutside(Vector3 worldPos)
    {
        HideInstant();

        ViewModel.PlaceCardHandler(worldPos, _buildObject);
    }

    // ===== АНИМАЦИИ =====

    private void Lift(float height)
    {
        _rect.DOKill();
        _rect.DOLocalMoveY(_startPos.y + height, 0.2f);
    }

    private void ResetPosition()
    {
        _rect.DOKill();
        _rect.DOLocalMove(_startPos, 0.2f);
    }

    private void Hide()
    {
        _canvasGroup.alpha = 0f;
    }

    private void HideInstant()
    {
        _canvasGroup.alpha = 0f;
    }

    private void Show()
    {
        _canvasGroup.alpha = 1f;
    }

    internal void UpdateGhost(object worldPos)
    {
        throw new NotImplementedException();
    }
}