using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class InventoryManager
{
    GamePlayer m_boundPlayer;
    HandSocket m_boundSocket;
    Transform ItemParent;
    InventoryUI m_invUI;
    Transform nonEquippableItemParent;

    public InventoryManager(GamePlayer player, HandSocket equippableSocket, Transform nonEquippablePoolParent)
    {
        m_boundPlayer = player;
        m_boundSocket = equippableSocket;
        nonEquippableItemParent = nonEquippablePoolParent;
        Init();
    }

    void Init()
    {
        m_invUI = GameManager.instance.GetInventoryUI();
    }

    public Equippable GetHandEquipped()
    {
        return m_invUI.GetEquipmentSlot(Equippable.EquippableType.HAND).Item as Equippable;
    }

    public bool AddToInventory(ItemBase itembs)
    {
        return m_invUI.AddToInventory(itembs);
    }

    public void RemoveLastItem()
    {
        InventorySlot lastEquipped = m_invUI.GetLastEquipped();
        if(lastEquipped != null)
        {
            m_invUI.RemoveItem(lastEquipped.Item);
        }
    }

    public Transform GetHandSocketTransform()
    {
        return m_boundSocket.GetSocketTransform();
    }

    public Transform getItemParent()
    {
        return nonEquippableItemParent;
    }

    void DropItem(ItemBase itembs)
    {
        
    }


    /*
    void EquipEvent(ItemBase item)
    {
        if(item is Equippable)
        {
            Equip(item as Equippable);
        }
        else
        {
            Debug.Log(item.GetIdentifier() + " is not euqippable.");
        }
    }
    /*

    public bool Equip(Equippable item)
    {
        try
        {
            EquipmentSlot neededSlot;
            equipmentSlots.TryGetValue((int)item.EquipmentType, out neededSlot);

            if (neededSlot.Item != null)
                return false;

            neededSlot.Item = item;
            RemoveFromInventory(item);
            neededSlot.OnLeftClickDequip += DequipEvent;
            item.OnEquip();
            //Debug.Log( neededSlot.name+" contains " +neededSlot.Item.GetIdentifier());
            return true;
        }
        catch(Exception e)
        {
            Debug.Log(e.ToString());
            return false;
        }
    }

    void RemoveFromInventory(ItemBase item)
    {
        inventorySpace.Remove(item);
        m_invUI.GetItemSlot(item).Item = null;
    }

    void DequipEvent(Equippable item)
    {
        Dequip(item);
    }

    public bool Dequip(Equippable item)
    {
        if (inventorySpace.Count < InventoryUI.inventorySize)
        {
            //AddToInventory(item);
            AddOrDrop(item);
            EquipmentSlot occupiedSlot;
            equipmentSlots.TryGetValue((int)item.EquipmentType, out occupiedSlot);
            occupiedSlot.OnLeftClickDequip -= DequipEvent;
            occupiedSlot.Item = null;
            item.OnDequip();
            return true;
        }
        return false;
    }

    public bool AddOrDrop(ItemBase item)
    {
        if(!AddToInventory(item))
        {
            item.transform.position = m_boundPlayer.transform.root.position;
            return false;
        }

        return true;
    }

    public bool AddToInventory(ItemBase itembs)
    {
        if(inventorySpace.Count < InventoryUI.inventorySize)
        {
            try
            {
                Equippable equip = (Equippable)itembs;
                if(equip.EquipmentType == Equippable.EquippableType.HAND)
                {
                    equip.transform.SetParent(m_boundSocket.GetSocketTransform());
                }
                else
                {
                    equip.transform.SetParent(m_boundPlayer.getAnimationController().GetClothParent());
                }
            }
            catch
            {
                itembs.transform.SetParent(m_boundPlayer.transform);
            }

            itembs.SetOwner(m_boundPlayer);
            inventorySpace.Add(itembs);

            InventorySlot freeSlot = m_invUI.GetFreeSlot();
            if(freeSlot.Item != null)
            {
                Debug.LogError("what the fuck?");
            }
            freeSlot.Item = itembs;

            itembs.OnAddedToInventory();
            itembs.Sleep();

            return true;
        }
        itembs.SetOwner(null);
        return false;
    }
    
    public bool RemoveLastItem()
    {
        if(inventorySpace.Count > 0)
        {
            ItemBase itemToRemove = inventorySpace[inventorySpace.Count-1];
            RemoveFromInventory(itemToRemove);
            return true;
        }
        return false;
    }
    */

}