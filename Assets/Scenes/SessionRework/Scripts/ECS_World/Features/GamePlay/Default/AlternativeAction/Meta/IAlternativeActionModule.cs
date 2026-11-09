namespace Scenes.SessionRework.Scripts.ECS_World.Features.GamePlay.Default.AlternativeAction.Meta
{
    public interface IAlternativeActionModule
    {
        void StartProcess();
        void UpdateProcess(float deltaTime);
        void StopProcess();
    }
}