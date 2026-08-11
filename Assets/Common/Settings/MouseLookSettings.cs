using UnityEngine;

public class MouseLookSettings
{
    public float Sensitivity { get; }
    public float TopClamp { get; }
    public float BottomClamp { get; }

    public MouseLookSettings(float sensitivity, float topClamp, float bottomClamp)
    {
        Sensitivity = sensitivity;
        TopClamp = topClamp;
        BottomClamp = bottomClamp;
    }
}