using UnityEngine;
using System.Collections.Generic;



public class Analytics : MonoBehaviour
{
    public static Analytics Instance = null;
    public static Queue<string> screensQueue = new Queue<string>();

    DefaultAnalytics connector;

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

    void OnDisable()
    {
        connector.StopSession();
    }


    public void init()
    {
        connector = gameObject.AddComponent<DefaultAnalytics>();
        connector.StartSession();
    }

    public static void TrackScreen(string screenName)
    {
        if (Analytics.Instance == null)
        {
            screensQueue.Enqueue(screenName);
        }
        else
        {
            Analytics.Instance.trackScreen(screenName);
        }
    }

    void trackScreen(string screenName)
    {
        connector.TrackScreen(screenName);
    }

    public void trackEvent(AnalyticsCategory category, AnalyticsAction action, string label, long value = 0)
    {
        trackEvent(category, action.ToString(), label, value);
    }

    public void trackEvent(AnalyticsCategory category, string action, string label, long value = 0)
    {
        connector.TrackEvent(category.ToString(), action, label, value);
    }



}
