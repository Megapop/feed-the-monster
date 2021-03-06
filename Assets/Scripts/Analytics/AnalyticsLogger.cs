﻿using System;
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
        FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);

        TrackEvent(FirebaseCustomEventNames.EventSessionInit);
    }


    /// <summary>
    /// Log an event to Firebase Analytics.
    /// </summary>
    public static void TrackEvent(string name, params Parameter[] parameters)
    {
        FirebaseAnalytics.LogEvent(name, parameters);
    }

    /// <summary>
    /// Track a specific level completed, with numerical value
    /// </summary>
    public static void TrackSpecificLevelComplete(int levelNum)
    {
        Analytics.Instance.TrackEvent(FirebaseCustomEventNames.EventLevelWinNumerated + levelNum.ToString(), 
            new Parameter(FirebaseCustomParameterNames.ParameterLevel, levelNum));
    }


    public static void TrackScene([NotNull] string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            throw new ArgumentException("Screen name null or empty", sceneName);
        }

        FirebaseAnalytics.SetCurrentScreen(sceneName, "scene");
    }
}