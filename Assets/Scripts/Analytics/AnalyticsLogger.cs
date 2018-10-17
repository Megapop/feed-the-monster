using System;
using Firebase.Analytics;
using JetBrains.Annotations;

/// <summary>
/// Logs analytics events.
/// </summary>
public static class AnalyticsLogger
{
    /// <summary>
    /// Custom Session tracking, backup of Firebase default (tracks when Analytics singleton inits).
    /// </summary>
    public static void OnSession()
    {
        LogToFirebase(FirebaseCustomEventNames.EventSessionInit);
    }


    /// <summary>
    /// Log an event to Firebase Analytics.
    /// </summary>
    private static void LogToFirebase(string name, params Parameter[] parameters)
    {
        FirebaseAnalytics.LogEvent(name, parameters);
    }


    public static void TrackScene([NotNull] string sceneName)
    {
        if (string.IsNullOrWhiteSpace(sceneName))
        {
            throw new ArgumentException("Screen name null or empty", nameof(sceneName));
        }

        FirebaseAnalytics.SetCurrentScreen(sceneName, "scene");
    }

    /*
    LogToFirebase(
        FirebaseCustomEventNames.EventTimeFreeze,
            new Parameter(FirebaseParameterNames.ParameterProgress, AnalyticsManager.CurrentLevelProgress),
            new Parameter(FirebaseAnalytics.ParameterLevel, CurrentLevel),
            new Parameter(FirebaseAnalytics.ParameterLevelName, CurrentLevelName),
            new Parameter(FirebaseParameterNames.ParameterLevelType, CurrentLevelType)
    );*/
}