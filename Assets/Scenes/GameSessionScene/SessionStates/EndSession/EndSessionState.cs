using Common.Services.Global;
using Common.systems.SceneStates.States;
using Common.systems.SceneStates.States.Attributes;
using Common.systems.UI;
using UnityEngine;
using Zenject;

[LinkToScene(typeof(GameSessionScene))]
public class EndSessionState : BaseState
{
    [Inject] private ThirdPersonCamera _camera;
    [Inject] private UIManager _uiManager;
    [Inject] private GlobalStorage _globalStorage;

    public override void EnterToState()
    {
        _uiManager.TryOpen("EndGameScrean", out object vm_obj);
        int waveNum = _globalStorage.Get<int>("WaveNum");

        if(vm_obj is EndGameViewModel viewModel)
        {
            viewModel.SetWaveNum(waveNum);
        }
    }
}
