using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Canvas inventoryCanvas;
    public Transform slotHolder;
    [SerializeField] InventorySlot[] slots;
    [SerializeField] Transform slotsParent;
    PlayerInventory localPlayerInvRef;

    void OnValidate()
    {
        if(slotsParent != null)
        {
            slots = slotsParent.GetComponentsInChildren<InventorySlot>();
        }
    }

    void Start()
    {
        if (GameManager.instance.getLocalPlayer() != null)
        {
            localPlayerInvRef = GameManager.instance.getLocalPlayer().getInventory();
        }
    }

    void  LateUpdate()
    {
        if(localPlayerInvRef == null)
        {
            localPlayerInvRef = GameManager.instance.getLocalPlayer().getInventory();
        }
        
        if (!inventoryCanvas.gameObject.activeInHierarchy)
            return;

        RefreshUI();
    }

    public void Clicked(int num)
    {
        
    }

    void RefreshUI()
    {
        ItemBase[] inventoryItems = localPlayerInvRef.GetItemInventory().ToArray();

        for (int i = 0; i < 20; i++) //20 is the number of slots.
        {
            if(i >= inventoryItems.Length)
            {
                slots[i].Item = null;
            }
            else
            {
                slots[i].Item = inventoryItems[i];
            }
        }
    }
}
