using System;
using UnityEngine;

public class SeagePhaseViewModel
{
    public Action<int> onWaveSeted;


    public void SetWave(int waveNum)
    {
        onWaveSeted?.Invoke(waveNum);
    }
}
