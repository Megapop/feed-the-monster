using UnityEngine;
using System.Collections;

public class UISubPanelController : MonoBehaviour
{
    public GameObject[] SubPanels;


    void OnEnable()
    {
        foreach (GameObject sub in SubPanels)
        {
            sub.SetActive(true);
        }
    }

    void OnDisable()
    {
        foreach (GameObject sub in SubPanels)
        {
            sub.SetActive(false);
        }
    }
}
