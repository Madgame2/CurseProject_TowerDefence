using TMPro;
using UnityEngine;

public class LobbyViewerElem : MonoBehaviour
{
    [SerializeField]private TMP_Text userNickName;
    [SerializeField] private GameObject extraOptionRoot;
    [SerializeField] private AvatarLoader avatar;

    public void Init(string nickName,string avatar, bool isHost)
    {
        userNickName.text = nickName;
        this.avatar.LoadFromUrl(avatar);

        if (isHost)
        {
            extraOptionRoot.SetActive(false);
        }
    }
}
