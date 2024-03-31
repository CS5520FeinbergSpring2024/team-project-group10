using System.Collections;
using UnityEngine;

public static class TimerManager
{
    public static IEnumerator StartCountdown(float duration, System.Action<float> onUpdate, System.Action onFinished)
    {
        float startTime = Time.realtimeSinceStartup;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            onUpdate?.Invoke(duration - elapsedTime);
            elapsedTime = Time.realtimeSinceStartup - startTime;
            yield return new WaitForSeconds(1f);
        }

        onFinished?.Invoke();
    }

    public static string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
