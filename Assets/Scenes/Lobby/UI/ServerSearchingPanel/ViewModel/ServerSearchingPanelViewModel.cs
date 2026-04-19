using System;
using UnityEngine;

public class SearchingPanelViewModel
{
    public Action<string> onTextChanged;

    public void SetNewText(string text)
    {
        onTextChanged?.Invoke(text);
    }

}
