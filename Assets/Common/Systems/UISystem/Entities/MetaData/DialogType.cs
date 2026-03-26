using UnityEngine;

namespace Common.systems.UI
{
    public enum DialogType
    {
        Neutral,   // обычный диалог
        Confirm,   // положительное действие (OK / Yes)
        Danger     // опасное действие (Delete / Exit / Reset)
    }
}
