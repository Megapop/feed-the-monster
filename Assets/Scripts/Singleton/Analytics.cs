using UnityEngine;
using System.Collections.Generic;

public class Analytics : MonoBehaviour
{
    public static Analytics Instance = null;
    public static Queue<string> screensQueue = new Queue<string>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SingletonLoader.CheckSingleton();
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void init()
    {
        AnalyticsLogger.OnSession();
    }

    public static void TrackScene(string sceneName)
    {
        if (Analytics.Instance == null)
        {
            screensQueue.Enqueue(sceneName);
        }
        else
        {
            AnalyticsLogger.TrackScene(sceneName);
        }
    }


    //TODO REPLACE WITH DIRECT CALLS TO APPROPRIATE EVENT IN ANALYTICS LOGGER
    public void trackEvent(AnalyticsCategory category, AnalyticsAction action, string label, long value = 0)
    {
        trackEvent(category, action.ToString(), label, value);
    }

    public void trackEvent(AnalyticsCategory category, string action, string label, long value = 0)
    {

    }
}