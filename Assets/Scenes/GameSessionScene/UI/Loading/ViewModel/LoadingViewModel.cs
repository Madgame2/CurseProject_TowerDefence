using System;
using UnityEngine;

namespace Scenes.Session
{
    public class LoadingViewModel
    {
        private SessionNetInstaller _netInstaller;

        public event Action<float> onProgressBarUpdate;
        public event Action<string> onStatusUpdated;

        internal void setNetInstaller(SessionNetInstaller netInstaller)
        {
            this._netInstaller = netInstaller;
            _netInstaller.onStageUpdate += handleUpdatedStage;
            _netInstaller.onProgressUpdate += handleProgressUpdate;

            onProgressBarUpdate?.Invoke(_netInstaller.CurrentProgress);
            onStatusUpdated?.Invoke(_netInstaller.Stage);

        }


        public void cleanUp()
        {
            _netInstaller.onStageUpdate -= handleUpdatedStage;
            _netInstaller.onProgressUpdate -= handleProgressUpdate;

        }

        private void handleUpdatedStage(string newStage)
        {
            onStatusUpdated?.Invoke(newStage);
        }
        private void handleProgressUpdate(float newValue)
        {
            onProgressBarUpdate?.Invoke(newValue);
        }

        internal void Init()
        {
            throw new NotImplementedException();
        }
    }
}
