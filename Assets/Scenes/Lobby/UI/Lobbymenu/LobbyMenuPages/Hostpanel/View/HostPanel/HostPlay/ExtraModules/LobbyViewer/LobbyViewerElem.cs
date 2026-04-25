using TMPro;
using UnityEngine;

public class LobbyViewerElem : MonoBehaviour
{
    [SerializeField]private TMP_Text userNickName;
    [SerializeField] private GameObject extraOptionRoot;

    public void Init(string nickName, bool isHost)
    {
        userNickName.text = nickName;

        if (isHost)
        {
            extraOptionRoot.SetActive(false);
        }
    }
}
