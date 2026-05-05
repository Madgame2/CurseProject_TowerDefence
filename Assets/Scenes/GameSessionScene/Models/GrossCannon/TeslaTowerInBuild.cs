using UnityEngine;
using UnityEngine.UI;

public class TeslaTowerInBuild : MonoBehaviour
{
    [SerializeField] private Canvas _uiCanvas;
    [SerializeField] private Slider _progressSlider;

    private void LateUpdate()
    {
        _uiCanvas.transform.rotation = Quaternion.LookRotation(
            transform.position - Camera.main.transform.position
        );
    }


    public void SetProgress(float progress)
    {
        this._progressSlider.value = progress;
    }
}
