using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerInventory
{
    [SerializeField]List<ItemBase> inventory;
    GamePlayer boundPlayer;
    HandSocket boundSocket;
    public Equippable currentlyEquipped;
    public const int inventorySize = 10;

    public PlayerInventory(GamePlayer player, HandSocket equippableSocket)
    {
        boundPlayer = player;
        boundSocket = equippableSocket;
        inventory = new List<ItemBase>();
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

        currentlyEquipped.ApplyOffsets();

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

    public bool AddToInventory(ItemBase item)
    {
        if(inventory.Count < 20)
        {
            inventory.Add(item);
            return true;
        }
        return false;
    }

    public bool RemoveItem(ItemBase item)
    {
        return inventory.Remove(item);
    }

    public List<ItemBase> GetItemInventory()
    {
        return inventory;
    }

    public bool RemoveLastItem()
    {
        if(inventory.Count > 0)
        {
            inventory.RemoveAt(inventory.Count -1);
            return true;
        }
        return false;
    }
}