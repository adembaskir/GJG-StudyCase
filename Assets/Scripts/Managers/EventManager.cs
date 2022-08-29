using UnityEngine;

public class EventManager : MonoBehaviour
{
    public delegate void LevelStarted();
    public static event LevelStarted onLevelStarted;

    public delegate void LevelCompleted();
    public static event LevelCompleted onLevelCompleted;

    public delegate void LevelFailed();
    public static event LevelFailed onLevelFailed;

    public delegate void FirstStageCompleted();
    public static event FirstStageCompleted firstStageCompleted;

    public void CallLevelStartedEvent()
    {
        if (onLevelStarted != null)
            onLevelStarted();
        Debug.Log("LevelStartedEvent");
    }
    public void CallLevelCompletedEvent()
    {
        if (onLevelCompleted != null)
            onLevelCompleted();
    }
    public void CallLevelFailedEvent()
    {
        if (onLevelFailed != null)
            onLevelFailed();
    }
    public void CallFirstStageComplatedEvent()
    {
        if (firstStageCompleted != null)
            firstStageCompleted();
    }

}



