using UnityEngine;
using System.Collections;

public class PauseMaker : MonoBehaviour
{
    void Awake()
    {

    }


    void OnEnable()
    {
        //GameplayController.Instance.IsPause = true;
        if (GameplayController.Instance != null)
        {
            GameplayController.Instance.IsPausePopup = true;
        }
    }

    void OnDisable()
    {
        //GameplayController.Instance.IsPause = false;
        if (GameplayController.Instance != null)
        {
            GameplayController.Instance.IsPausePopup = false;
        }
    }
}
