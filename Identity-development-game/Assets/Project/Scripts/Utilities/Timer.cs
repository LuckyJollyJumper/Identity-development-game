using UnityEngine;

public class Timer : MonoBehaviour
{
    private float ElapsedTime = 0f;
    private bool isRunning = false;
    private float Duration = 0f;
    private bool UsesDuration = false;

    // ------------------------------------------------------------------
    // Convenience helpers for yielding
    // ------------------------------------------------------------------

    /// <summary>
    /// Static coroutine that simply waits for <paramref name="seconds"/> before returning.
    /// This allows other scripts to do <c>StartCoroutine(Timer.Delay(2f));</c> without
    /// needing a Timer component instance.
    /// </summary>
    public static System.Collections.IEnumerator Delay(float seconds)
    {
        float t = 0f;
        while (t < seconds)
        {
            t += Time.deltaTime;
            yield return null;
        }
    }

    /// <summary>
    /// Instance coroutine: starts a duration timer and yields until it finishes.
    /// Useful when you already have a Timer component and want to block further
    /// execution in a coroutine until <paramref name="seconds"/> have passed.
    /// </summary>
    public System.Collections.IEnumerator WaitForDuration(float seconds)
    {
        StartTimerWithDuration(seconds);
        while (!IsFinished())
        {
            yield return null;
        }
    }

    /// <summary>
    /// Waits for the remaining time on a previously configured duration timer.
    /// Call <c>StartTimerWithDuration</c> beforehand.
    /// </summary>
    public System.Collections.IEnumerator WaitRemaining()
    {
        while (UsesDuration && !IsFinished())
        {
            yield return null;
        }
    }

    // ------------------------------------------------------------------

    /// <summary>
    /// Starts the timer.
    /// </summary>
    public void StartTimer(){
        this.isRunning = true;
        this.ElapsedTime = 0f;
    }

    /// <summary>
    /// Starts a timer with a specific duration (countdown timer).
    /// </summary>
    public void StartTimerWithDuration(float durationInSeconds){
        this.isRunning = true;
        this.ElapsedTime = 0f;
        this.Duration = durationInSeconds;
        this.UsesDuration = true;
    }

    /// <summary>
    /// Stops the timer.
    /// </summary>
    public void Stop(){
        this.isRunning = false;
    }

    /// <summary>
    /// Resets the timer to zero.
    /// </summary>
    public void Reset(){
        ElapsedTime = 0f;
        isRunning = false;
    }

    /// <summary>
    /// Gets the elapsed time in seconds.
    /// </summary>
    public float GetElapsedTime(){
        return ElapsedTime;
    }

    /// <summary>
    /// Gets the remaining time for a duration-based timer.
    /// </summary>
    public float GetRemainingTime(){
        if (!UsesDuration) return 0f;
        return Mathf.Max(0f, Duration - ElapsedTime);
    }

    /// <summary>
    /// Checks if the timer is currently running.
    /// </summary>
    public bool IsRunning(){
        return this.isRunning;
    }

    /// <summary>
    /// Checks if a duration-based timer has finished.
    /// </summary>
    public bool IsFinished(){
        return UsesDuration && ElapsedTime >= Duration;
    }

    /// <summary>
    /// Gets the progress of a duration-based timer (0 to 1).
    /// </summary>
    public float GetProgress(){
        if (!UsesDuration || Duration <= 0f) return 0f;
        return Mathf.Clamp01(ElapsedTime / Duration);
    }

    private void Update(){
        if (this.isRunning){
            ElapsedTime += Time.deltaTime;

            // Stop if duration timer has finished
            if (UsesDuration && ElapsedTime >= Duration){
                this.isRunning = false;
            }
        }
    }
}
