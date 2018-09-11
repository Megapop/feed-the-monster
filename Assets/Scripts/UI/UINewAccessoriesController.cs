using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UINewAccessoriesController : MonoBehaviour
{
    public Image itemImage;
    public AudioClip audioClip;


    void Start()
    {
        addItem();

        UsersController.Instance?.userData()?.resetNewAvailableAccessories();


        AudioController.Instance?.PlaySound(audioClip);
    }


    void addItem()
    {
        bool isIcon = false;
        int itemId = UsersController.Instance?.userData()?.getNewAvailableAccessorie() ?? 0;
        if (itemId > 0)
        {
            var sprite = Resources.Load<Sprite>("Gameplay/Dressing/Items/Item_" + itemId.ToString());
            if (sprite)
            {
                itemImage.sprite = sprite;
                isIcon = true;
            }
        }

        if (!isIcon)
        {
            itemImage.gameObject.SetActive(false);
        }
    }

    public void OnCloseClick()
    {
        Destroy(gameObject);
    }
}
