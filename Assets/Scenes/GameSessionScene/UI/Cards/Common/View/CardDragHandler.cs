using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;


public class CardDragHandler : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerDownHandler,
    IPointerUpHandler,
    IDragHandler
{
    [Header("Refs")]
    [SerializeField] private RectTransform _rect;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private RectTransform _dragZone;
    [SerializeField] private LayerMask raycastMask;

    [Header("Link")]
    [SerializeField] private CardView _cardView; 

    private Vector3 _startPos;
    private bool _isDragging;
    private bool _isSelected;

    private void Awake()
    {
        _startPos = _rect.localPosition;
    }

    // ===================== HOVER =====================

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_isDragging) return;
        _cardView.OnHoverEnter();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_isDragging) return;
        _cardView.OnHoverExit();
    }

    // ===================== CLICK =====================

    public void OnPointerDown(PointerEventData eventData)
    {


        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        _isDragging = true;
        _isSelected = true;

        _cardView.OnSelect();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        _isDragging = false;
        _isSelected = false;

        bool inside = IsInsideZone();

        if (inside)
        {
            _cardView.OnReleasedInside();
            ResetPosition();
        }
        else
        {
            Vector3 worldPos = GetWorldPoint(eventData);

            _cardView.OnReleasedOutside(worldPos);
            ResetPositionInstant();
        }
    }

    // ===================== DRAG =====================

    public void OnDrag(PointerEventData eventData)
    {
        if (!_isDragging) return;

        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _rect.parent as RectTransform,
            eventData.position,
            null,
            out pos
        );

        _rect.localPosition = pos;



        bool inside = IsInsideZone();

        if (inside)
            _cardView.OnDragInside();
        else
        {
            _cardView.OnDragOutside();

            Vector3 worldPos = GetWorldPoint(eventData);
            _cardView.UpdateGhost(worldPos);
        }
    }

    private Vector3 GetWorldPoint(PointerEventData eventData)
    {
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, raycastMask))
            return hit.point;

        return Vector3.zero;
    }

    // ===================== ZONE =====================

    private bool IsInsideZone()
    {
        if (_dragZone == null) return true;

        return RectTransformUtility.RectangleContainsScreenPoint(
            _dragZone,
            _rect.position,
            null
        );
    }

    // ===================== HELPERS =====================

    private void ResetPosition()
    {
        _rect.DOKill();
        _rect.DOLocalMove(_startPos, 0.2f);
    }

    private void ResetPositionInstant()
    {
        _rect.localPosition = _startPos;
        _canvasGroup.alpha = 1;
    }
}