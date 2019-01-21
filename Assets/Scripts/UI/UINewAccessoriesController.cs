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

        if (UsersController.Instance && UsersController.Instance.userData() != null)
        {
            UsersController.Instance.userData().resetNewAvailableAccessories();
        }

        if (AudioController.Instance)
        {
            AudioController.Instance.PlaySound(audioClip);
        }
    }


    void addItem()
    {
        bool isIcon = false;
        int itemId = 0;

        if (UsersController.Instance && UsersController.Instance.userData() != null)
        {
            itemId = UsersController.Instance.userData().getNewAvailableAccessorie();
        }

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
