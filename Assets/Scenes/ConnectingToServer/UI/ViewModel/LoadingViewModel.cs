using System;
using UnityEngine;

public class LoadingViewModel
{
    public event Action<string> onMessageUpdate;

    public void SetMessage(string message)
    {
        onMessageUpdate?.Invoke(message);
    }
}
