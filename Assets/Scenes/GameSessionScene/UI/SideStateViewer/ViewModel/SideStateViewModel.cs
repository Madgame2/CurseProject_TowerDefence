using System;
using UnityEngine;

public class SideStateViewModel
{
    private int maxBuild = 4;
    private int currentBuild = 0;

    private int currentWave = 0;

    // События
    public Action<int>? OnCurrentBuildChanged;
    public Action<int>? OnMaxBuildChanged;

    public Action<int>? OnWaveChanged;

    public void SetBuildCount(int buildCount, int maxCount)
    {
        if (currentBuild != buildCount)
        {
            currentBuild = buildCount;
            OnCurrentBuildChanged?.Invoke(currentBuild);
        }

        if (maxBuild != maxCount)
        {
            maxBuild = maxCount;
            OnMaxBuildChanged?.Invoke(maxBuild);
        }
    }

    internal void SetWave(int? wave)
    {
        if (currentWave != wave)
        {
            currentWave = wave.Value;
            OnWaveChanged?.Invoke(currentWave);
        }
    }
}