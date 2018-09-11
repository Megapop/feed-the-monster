using UnityEngine;

public class DefaultAnalytics : MonoBehaviour
{
    public void StopSession()
    {
        Debug.LogWarning("Implement StopSession() in Firebase.");
    }

    public void StartSession()
    {
        Debug.LogWarning("Implement StartSession() in Firebase.");
    }

    public void TrackScreen(string screenName)
    {
        Debug.LogWarning("Implement TrackScreen() in Firebase.");
    }

    public void TrackEvent(string category, string action, string label, long value)
    {
        Debug.LogWarning("Implement TrackWarning() in Firebase.");
    }

}
