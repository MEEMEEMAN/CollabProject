using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlot : InventorySlot
{
    public override void OnValidate()
    {
        if (image == null)
        {
            image = GetComponentInChildren<Image>();
        }
    }
}
