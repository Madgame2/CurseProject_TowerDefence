using TMPro;
using UnityEngine;
using UnityEngine.UI;


using System;


public class LobbyViewerElem : MonoBehaviour
{
    [SerializeField] private TMP_Text userNickName;
    [SerializeField] private AvatarLoader avatar;
    [SerializeField] private Button removeUser;
    [SerializeField] private GameObject extraOptionRoot;

    private string id;
    private Action<string> onRemoveCallback;

    public void Init(
        string id,
        string nickName,
        string avatarUrl,
        bool isHost,
        Action<string> onRemove)
    {
        userNickName.text = nickName;
        this.id = id;

        avatar.LoadFromUrl(avatarUrl);

        onRemoveCallback = onRemove;

        if (isHost)
        {
            extraOptionRoot.SetActive(false);
        }
        else
        {
            removeUser.onClick.AddListener(HandleRemoveUser);
        }
    }

    private void HandleRemoveUser()
    {
        onRemoveCallback?.Invoke(id);
    }

    private void OnDisable()
    {
        removeUser.onClick.RemoveListener(HandleRemoveUser);
        onRemoveCallback = null;
    }
}