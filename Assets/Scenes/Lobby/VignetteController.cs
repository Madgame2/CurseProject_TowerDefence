using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using DG.Tweening;

public class VignetteController : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public float duration = 1f;


    public Tween FadeOut()
    {
        return canvasGroup.DOFade(1f, duration).SetEase(Ease.InOutSine);
    }

    public Tween FadeIn()
    {
        canvasGroup.gameObject.SetActive(true);
        return canvasGroup.DOFade(0f, duration)
            .SetEase(Ease.InOutSine);
    }
}
