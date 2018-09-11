using UnityEngine;
using System.Collections;

public class UIFlashAnimation : MonoBehaviour
{
    public delegate void onDelegate();
    public onDelegate onEnd;
    public onDelegate onPeak;

    public AudioClip sndEvolv;


    void Start()
    {
        StartCoroutine(ChangeTimer());

        AudioController.Instance.PlaySound(sndEvolv);
    }


    IEnumerator ChangeTimer()
    {
        yield return new WaitForSeconds(0.1f);
        ChangeScreen();

        yield return new WaitForSeconds(2.5f);
        End();
    }


    public void ChangeScreen()
    {
        if (onPeak != null)
        {
            onPeak();
        }
    }

    public void End()
    {
        if (onEnd != null)
        {
            onEnd();
        }
    }
}
