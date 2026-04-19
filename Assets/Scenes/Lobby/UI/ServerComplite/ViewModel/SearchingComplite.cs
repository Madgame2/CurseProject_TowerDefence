using System;
using UnityEngine;

public class SearchingCompliteViewModle
{
    public Action<string> onTextChanged;

    public void SetNewText(string text)
    {
        onTextChanged?.Invoke(text);
    }

}
