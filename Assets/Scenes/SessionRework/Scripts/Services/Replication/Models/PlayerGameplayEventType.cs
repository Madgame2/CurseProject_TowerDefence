namespace Scenes.SessionRework.Scripts.Services.Replication.Models
{
    public enum PlayerGameplayEventType : byte
    {
        InstantAction = 1,

        ContinuousActionStarted = 2,
        ContinuousActionEnded = 3,
        ContinuousActionUpdated = 4,
        ContinuousActionCancelled = 5
    }
}