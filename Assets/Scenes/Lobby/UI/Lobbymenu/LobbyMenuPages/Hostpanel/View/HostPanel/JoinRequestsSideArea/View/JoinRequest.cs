using Common.systems.ProfileSystem.Entities;
using System;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.UI;

public class JoinRequest : MonoBehaviour
{
    [SerializeField] private TMP_Text _playerNicknameArea;
    [SerializeField] private Image _playerAvatar;
    [SerializeField] private Button _rejectButton;
    [SerializeField] private Button _applyButton;
    [SerializeField] private Slider _TTlSlider;

    private Profile _profile;
    private string RequestId;

    private float _maxTTL;
    private float _currentTTL;
    private bool _isInitialized;

    public event Action<Profile, string> onApplyPlayerRequest;

    public void Init(Profile profile,string requestId , float TTL)
    {
        _profile = profile;
        RequestId = requestId;
        _playerNicknameArea.text = profile.ProfileName;

        _maxTTL = TTL;
        _currentTTL = TTL;

        _TTlSlider.value = 1f;

        _isInitialized = true;

        _rejectButton.onClick.AddListener(Expire);
        _applyButton.onClick.AddListener(ApplyHandler);
    }


    private void Update()
    {
        if (!_isInitialized) return;

        _currentTTL -= Time.deltaTime;
        _TTlSlider.value = _currentTTL / _maxTTL;

        if (_currentTTL <= 0f)
        {
            Expire();
        }
    }

    private void ApplyHandler()
    {
        onApplyPlayerRequest?.Invoke(_profile, RequestId);
        Expire();
    }

    private void Expire()
    {
        if (!_isInitialized) return; 

        _isInitialized = false;

        Cleanup();

        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        Cleanup();
    }

    private void Cleanup()
    {
        _rejectButton.onClick.RemoveAllListeners();
        _applyButton.onClick.RemoveAllListeners();

        onApplyPlayerRequest = null;
    }
}
