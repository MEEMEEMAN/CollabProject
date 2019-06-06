using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerInventory
{
    GamePlayer boundPlayer;
    HandSocket boundSocket;
    public Equippable currentlyEquipped;
    public const int inventorySize = 10;
    InventorySlot[] inventory;
    
    struct InventorySlot
    {
        public ItemBase item;
    }

    public PlayerInventory(GamePlayer player ,HandSocket value)
    {
        boundPlayer = player;
        boundSocket = value;
        inventory = new InventorySlot[inventorySize];
    }

    public bool MoveToHands(Equippable equipment)
    {
        if (currentlyEquipped == null)
        {
            currentlyEquipped = equipment;
            EquippedToHand();
            return true;
        }
        else
        {
            equipment.DestroyItem();
            return false;
        }
    }

    void EquippedToHand()
    {
        currentlyEquipped.transform.SetParent(boundSocket.getTransform());

        currentlyEquipped.transform.localPosition = currentlyEquipped.offsetData.localPosition;
        currentlyEquipped.transform.localRotation = currentlyEquipped.offsetData.localRotation;
        currentlyEquipped.transform.localScale = currentlyEquipped.offsetData.localScale;
        if (currentlyEquipped.colliders != null)
        {
            currentlyEquipped.colliders.gameObject.SetActive(false);
            currentlyEquipped.GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    public void Dequip()
    {
        if (currentlyEquipped != null)
        {
            currentlyEquipped.DestroyItem();
            currentlyEquipped = null;
        }
    }

    public Equippable getCurrentlyEquipped()
    {
        return currentlyEquipped;
    }
}