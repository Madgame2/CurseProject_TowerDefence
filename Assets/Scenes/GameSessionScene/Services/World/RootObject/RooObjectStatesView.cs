using UnityEngine;
using UnityEngine.UI;

public class RooObjectStatesView : MonoBehaviour
{
    [SerializeField] private Canvas _uiCanvas;
    [SerializeField] private Slider _progressSlider;

    private void LateUpdate()
    {
        _uiCanvas.transform.rotation = Quaternion.LookRotation(
            transform.position - Camera.main.transform.position
        );
    }


    public void SetHealth(float Health)
    {
        this._progressSlider.value = Health;
    }
}
