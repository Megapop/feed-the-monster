using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultAnalitics : MonoBehaviour, IAnalitics
{

    public void init()
    {
        //GameObject goLoader = Instantiate (Resources.Load ("Gameplay/GAv4") as GameObject);
        Instantiate(Resources.Load("Gameplay/GAv4") as GameObject);
    }

    public void StopSession()
    {
        Debug.LogWarning("Implement StopSession() in Firebase.");
    }

    public void StartSession()
    {
        Debug.LogWarning("Implement StartSession() in Firebase.");
    }

    public void TreckScreen(string screenName)
    {
        Debug.LogWarning("Implement TreckScreen() in Firebase.");
    }

    public void TreckEvent(string category, string action, string label, long value)
    {
        Debug.LogWarning("Implement TrackWarning() in Firebase.");
    }

}
