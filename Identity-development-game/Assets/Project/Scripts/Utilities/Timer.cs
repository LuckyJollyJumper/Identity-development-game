using UnityEngine;

public class Timer : MonoBehaviour
{
    private float elapsedTime = 0f;
    private bool isRunning = false;
    private float duration = 0f;
    private bool usesDuration = false;

    /// <summary>
    /// Starts the timer.
    /// </summary>
    public void StartTimer(){
        isRunning = true;
        elapsedTime = 0f;
    }

    /// <summary>
    /// Starts a timer with a specific duration (countdown timer).
    /// </summary>
    public void StartTimerWithDuration(float durationInSeconds){
        isRunning = true;
        elapsedTime = 0f;
        duration = durationInSeconds;
        usesDuration = true;
    }

    /// <summary>
    /// Stops the timer.
    /// </summary>
    public void Stop(){
        isRunning = false;
    }

    /// <summary>
    /// Pauses the timer (can be resumed with Resume()).
    /// </summary>
    public void Pause(){
        isRunning = false;
    }

    /// <summary>
    /// Resumes a paused timer.
    /// </summary>
    public void Resume(){ isRunning = true; }

    /// <summary>
    /// Resets the timer to zero.
    /// </summary>
    public void Reset(){
        elapsedTime = 0f;
        isRunning = false;
    }

    /// <summary>
    /// Gets the elapsed time in seconds.
    /// </summary>
    public float GetElapsedTime(){
        return elapsedTime;
    }

    /// <summary>
    /// Gets the remaining time for a duration-based timer.
    /// </summary>
    public float GetRemainingTime(){
        if (!usesDuration) return 0f;
        return Mathf.Max(0f, duration - elapsedTime);
    }

    /// <summary>
    /// Checks if the timer is currently running.
    /// </summary>
    public bool IsRunning(){
        return isRunning;
    }

    /// <summary>
    /// Checks if a duration-based timer has finished.
    /// </summary>
    public bool IsFinished(){
        return usesDuration && elapsedTime >= duration;
    }

    /// <summary>
    /// Gets the progress of a duration-based timer (0 to 1).
    /// </summary>
    public float GetProgress(){
        if (!usesDuration || duration <= 0f) return 0f;
        return Mathf.Clamp01(elapsedTime / duration);
    }

    private void Update(){
        if (isRunning){
            elapsedTime += Time.deltaTime;

            // Stop if duration timer has finished
            if (usesDuration && elapsedTime >= duration){
                isRunning = false;
            }
        }
    }
}
