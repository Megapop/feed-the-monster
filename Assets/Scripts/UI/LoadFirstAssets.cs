using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadFirstAssets : MonoBehaviour
{
    public GameObject baseBackground;


    void Start()
    {
        Monster[] monsters = UsersController.Instance.userData().getCollectedMonsters;
        //GameObject go = Resources.Load ("Gameplay/Background/BG_2") as GameObject;
    }
}
