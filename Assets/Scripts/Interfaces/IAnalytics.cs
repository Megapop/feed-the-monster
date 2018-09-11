public interface IAnalytics
{
    void StopSession();
    void StartSession();
    void TrackScreen(string screenName);
    void TrackEvent(string category, string action, string label, long value);
}
