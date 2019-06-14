using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour
{
    public Image image;
    Sprite slotImage = null;
    ItemBase _item;

    public ItemBase Item
    {
        get{ return _item; }
        set
        {
            _item = value;
            if(slotImage == null)
            slotImage = image.sprite;

            if(_item == null)
            {
                image.sprite = slotImage;
            }
            else
            {
                if (Item.itemImage != null)
                    image.sprite = _item.itemImage;
                else
                    image.sprite = GameManager.instance.GetUIManager().getDefaultSprite();
            }

        }
    }

    public virtual void OnValidate()
    {
        if(image == null)
        {
            image = GetComponent<Image>();
        }
    }

}
