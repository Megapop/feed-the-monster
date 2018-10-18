using UnityEngine;
using System.Collections.Generic;
using Firebase.Analytics;

public class Analytics : MonoBehaviour
{
    public static Analytics Instance = null;
    public static Queue<string> scenesQueue = new Queue<string>();
    public static Queue<string> eventsQueue = new Queue<string>();


    /// <summary>
    /// Initiate singleton
    /// </summary>
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

    /// <summary>
    /// If anything has been queued of events/scene tracking before singleton initiation, track everything that has been accumulated in the queue.
    /// </summary>
    public void init()
    {
        AnalyticsLogger.OnSession();

        while (scenesQueue.Count > 0)
        {
            AnalyticsLogger.TrackScene(scenesQueue.Dequeue());
        }

        while (eventsQueue.Count > 0)
        {
            AnalyticsLogger.TrackEvent(eventsQueue.Dequeue());
        }
    }

    /// <summary>
    /// Track scene, first ensure Analytics has been init
    /// </summary>
    public void TrackScene(string sceneName)
    {
        Debug.Log("SCENE: " + sceneName);

        if (Instance == null)
        {
            scenesQueue.Enqueue(sceneName);
        }
        else
        {
            AnalyticsLogger.TrackScene(sceneName);
        }
    }

    /// <summary>
    /// Track event, first ensure Analytics has been init
    /// </summary>
    public void TrackEvent(string eventName, params Parameter[] parameters)
    {
        Debug.Log("EVENT: " + eventName);

        if (Instance == null)
        {
            eventsQueue.Enqueue(eventName);
        }
        else
        {
            AnalyticsLogger.TrackEvent(eventName, parameters);
        }
    }
}