using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class PreparationPhaseViewModel
{
    private int countdown;

    public event Action<int> OnTimerChanged;

    public event Action OnTimerFinished;

    private CancellationTokenSource cancellationToken;

    public void SetTimer(int time)
    {
        countdown = time;

        cancellationToken?.Cancel();

        cancellationToken = new CancellationTokenSource();

        _ = StartTimer(cancellationToken.Token);
    }


    private async Task StartTimer(CancellationToken token)
    {
        OnTimerChanged?.Invoke(countdown);

        while (countdown > 0)
        {
            try
            {
                await Task.Delay(1000, token);
            }
            catch (TaskCanceledException)
            {
                return;
            }

            countdown--;

            OnTimerChanged?.Invoke(countdown);
        }

        OnTimerFinished?.Invoke();
    }
}
