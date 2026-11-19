using System;
using UnityEngine;

public class TimerScript : MonoBehaviour
{
    public static Action<string> AnnounceTimeDigital;
    public static Action<float> AnnounceTimeAnalog;
    public static Action AnnounceTimeCompletion;

    [Header("Required")]
    [SerializeField] private float totalTimeInSecs;

    [Header("Display As In Universe Time")]
    [SerializeField] private bool displayInUniverse;
    [SerializeField] private int dayStartInMinutes;
    [SerializeField] private int dayEndInMinutes;
    [SerializeField] private bool returnDigital = true;
    [SerializeField] private bool returnAnalog = false;

    private float currentTime = 0;
    private float startTime;
    private bool timerGoing = false;

    private float inUniverseTimeMultiplier;

    private void Start()
    {
        if (totalTimeInSecs == 0)
        {
            Debug.Log("No time has been set");
        }

        ResetTimer();
        StartTimer();

        if (displayInUniverse) CalculateMultiplier();
    }

    private void Update()
    {
        if (!timerGoing) return;

        currentTime = Time.time - startTime;
        if (displayInUniverse) ConvertToInUniverse();
        if (currentTime >= totalTimeInSecs) TimeIsUp();
    }

    private void StartTimer()
    {
        ResetTimer();
        timerGoing = true;
    }

    private void StopTimer()
    {
        timerGoing = false;
    }

    private void ResetTimer()
    {
        currentTime = 0;
        startTime = Time.time;
    }

    private void TimeIsUp()
    {
        Debug.Log("Time!");
        StopTimer();

        try
        {
            AnnounceTimeCompletion.Invoke();
        }
        catch (Exception e)
        {
            Debug.LogWarning("Problem in the Timer Script. Message: " + e.Message);
        }
    }    

    private void CalculateMultiplier()
    {
        float inUniTimeTaken = dayEndInMinutes - dayStartInMinutes;
        inUniverseTimeMultiplier = inUniTimeTaken / totalTimeInSecs;
    }

    private void ConvertToInUniverse()
    {
        float universeMinutesPassed = inUniverseTimeMultiplier * currentTime;

        int universeTime = dayStartInMinutes + Mathf.FloorToInt(universeMinutesPassed);

        try
        {
            if (returnDigital) AnnounceTimeDigital.Invoke(FormatInUniverse(universeTime));
        }
        catch (Exception e)
        {
            Debug.LogWarning("Problem in the Timer Script. Message: " + e.Message);
        }

        try
        {
            if (returnAnalog) AnnounceTimeAnalog.Invoke(FormatIntoDegrees(universeTime));
        }
        catch (Exception e)
        {
            Debug.LogWarning("Problem in the Timer Script. Message: " + e.Message);
        }
    }

    private string FormatInUniverse(int timeToFormat)
    {
        int leftSide = Mathf.FloorToInt(timeToFormat / 60);
        int rightSide = Mathf.FloorToInt(timeToFormat % 60);

        return (leftSide.ToString("D2") + ":" + rightSide.ToString("D2"));
    }

    private float FormatIntoDegrees(int timeToFormat)
    {
        return (720 * (timeToFormat / 1440));
    }
}
