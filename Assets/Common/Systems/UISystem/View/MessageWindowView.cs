using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MessageWindowView : MonoBehaviour
{
    [SerializeField] private TMP_Text _title;
    [SerializeField] private TMP_Text _description;
    [SerializeField] private Button _confirmButton;

    public void onConfirm()
    {
        Destroy(this.gameObject);
    }

    public void Init(string Tittle, string description)
    {
        _title.text = Tittle;
        _description.text = description;
    }
}
