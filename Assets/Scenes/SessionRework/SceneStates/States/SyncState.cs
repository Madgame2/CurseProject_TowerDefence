using Common.Services.Net.Modules;
using Common.systems.SceneStates.States;
using Common.systems.SceneStates.States.Attributes;
using Scenes.SessionRework;
using UnityEngine;
using Zenject;

[LinkToScene(typeof(SessionReworkScene))]
public class SyncState: BaseState
{
    [Inject] private readonly WebSocketModule _webSocketModule;
    
    
    public override void EnterToState()
    {
        _ = _webSocketModule.Send("ReadyToSync", null);
    }

    public override void LeaveFormState()
    {
        
    }
}
