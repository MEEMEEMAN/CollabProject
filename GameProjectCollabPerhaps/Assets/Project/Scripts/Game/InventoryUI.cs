using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InventoryUI : MonoBehaviour
{
    [Header("Equipment")]
    [SerializeField] Transform equipSlotParent;
    [SerializeField] EquipmentSlot[] equipSlots;
    [Header("Inventory")]
    [SerializeField] Transform slotsParent;
    [SerializeField] InventorySlot[] invSlots;
    [Header("Other")]
    public Canvas inventoryCanvas;
    public Transform slotHolder;
    public const int inventorySize = 20;
    public Transform onDragTransformParent;
    [Header("Items")]
    public static Dictionary<ItemBase, InventorySlot> itemPossesion;
    public static event Action<Equippable> EquipEvent;

    [SerializeField] GamePlayer UIOwner;

    public void SetOwner(GamePlayer player)
    {
        UIOwner = player;
    }

    void OnValidate()
    {
        if(slotsParent != null)
        invSlots = slotsParent.GetComponentsInChildren<InventorySlot>();

        if(equipSlotParent != null)
        {
            equipSlots = equipSlotParent.GetComponentsInChildren<EquipmentSlot>();
        }
    }

    Dictionary<int, EquipmentSlot> equipIndex;
    void Start()
    {
        if (GameManager.instance.getLocalPlayer() != null)
        {
            UIOwner = GameManager.instance.getLocalPlayer();
        }

        equipIndex = new Dictionary<int, EquipmentSlot>();

        foreach (EquipmentSlot slot in equipSlots)
        {
            equipIndex.Add((int)slot.EquipType, slot);
            slot.DoubleClickInteraction += DoubleClickEvent;
            slot.ItemDropInteraction += DropEvent;
        }

        itemPossesion = new Dictionary<ItemBase, InventorySlot>();

        foreach (InventorySlot slot in invSlots)
        {
            slot.DoubleClickInteraction += DoubleClickEvent;
            slot.ItemDropInteraction += DropEvent;
        }
    }

    public bool RemoveItem(ItemBase item)
    {
        InventorySlot slotToKill;
        itemPossesion.TryGetValue(item, out slotToKill);
        if(slotToKill != null)
        {
            ItemBase killed = slotToKill.Item;
            slotToKill.Item = null;
            itemPossesion.Remove(killed);
            killed.DestroyItem();
            return true;
        }
        return false;
    }

    void LateUpdate()
    {
        
        if(UIOwner == null)
        {
            UIOwner = GameManager.instance.getLocalPlayer();
        }
        
        if (!inventoryCanvas.gameObject.activeInHierarchy)
            return;
    }

    void DoubleClickEvent(InventorySlot slot)
    {
        if (slot == null)
            return;

        if(slot is EquipmentSlot) //if adding to inventory
        {
            if(slot.Item != null)
            {
                InventorySlot freeslot = GetFreeInvSlot();
                if (freeslot == null)
                    return;
                InsertSafely(slot, freeslot);
            }
        }
        else // if adding to equipment
        {
            if (slot.Item != null) 
            {
                if (slot.Item is Equippable)
                {
                    EquipmentSlot equipSlot = GetEquipmentSlot(((Equippable)slot.Item).EquipmentType);

                    InsertSafely(slot, equipSlot);
                }
            }
        }
    }

    static bool InsertSafely(InventorySlot toInsert, InventorySlot insertLocation)
    {
        if (toInsert == null || insertLocation == null || toInsert.Item == null || insertLocation.Item != null)
            return false;

        if (toInsert is InventorySlot && insertLocation is EquipmentSlot) //if equippable is inserted to equipment slot
        {
            if (((EquipmentSlot)insertLocation).EquipType == ((Equippable)toInsert.Item).EquipmentType)
            {
                toInsert = Insert(toInsert, insertLocation);
                ((Equippable)toInsert.Item).OnEquip();
                EquipEvent(toInsert.Item as Equippable);
                return true;
            }

            return false;
        }

        if (toInsert is EquipmentSlot && insertLocation is InventorySlot) //if equippable is moved from equipment slot back to inventory
        {
            toInsert = Insert(toInsert, insertLocation);
            ((Equippable)toInsert.Item).OnDequip();
            EquipEvent(null);
            return true;
        }

        if (toInsert is InventorySlot && insertLocation is InventorySlot)  //if simply moving the item in the inventory
        {
            Insert(toInsert, insertLocation);
            return true;
        }
        
        return false;
    }

    public static bool CanDrop(InventorySlot dragged, InventorySlot droppedon)
    {
        if (dragged == null || droppedon == null)
            return false;

        if(dragged.Item != null && droppedon.Item == null)
        {
            if (dragged is InventorySlot && droppedon is EquipmentSlot) //if equippable is inserted to equipment slot
            {
                if (((EquipmentSlot)droppedon).EquipType == ((Equippable)dragged.Item).EquipmentType)
                {
                    return true;
                }

                return false;
            }

            if (dragged is EquipmentSlot && droppedon is InventorySlot) //if equippable is moved from equipment slot back to inventory
            {
                return true;
            }

            if (dragged is InventorySlot && droppedon is InventorySlot)  //if simply moving the item in the inventory
            {
                return true;
            }
        }

        return false;
    }

    void DropEvent(InventorySlot dragged, InventorySlot droppedOn)
    {
        InsertSafely(dragged, droppedOn);
    }

    static InventorySlot Insert(InventorySlot toInsert, InventorySlot insertLocation)
    {
        insertLocation.Item = toInsert.Item;
        toInsert.Item = null;

        itemPossesion[insertLocation.Item] = insertLocation;

        return insertLocation;
    }

    public EquipmentSlot GetEquipmentSlot(Equippable.EquippableType type)
    {
        EquipmentSlot slot;
        equipIndex.TryGetValue((int)type, out slot);
        return slot;
    }

    public InventorySlot GetFreeInvSlot()
    {
        for (int i = 0; i < invSlots.Length; i++)
        {
            if(invSlots[i].Item == null)
            {
                return invSlots[i];
            }
        }
        return null;
    }

    public InventorySlot GetLastEquipped()
    {
        for (int i = invSlots.Length-1; i >= 0; i--)
        {
            if(invSlots[i].Item != null)
            {
                return invSlots[i];
            }
        }
        return null;
    }

    public bool AddToInventory(ItemBase itembs)
    {
        itembs = itembs.Create();

        InventorySlot freeslot = GetFreeInvSlot();

        if (freeslot == null)
        {
            itembs.transform.position = UIOwner.transform.position;
            return false;
        }

        itembs.SetOwner(UIOwner);
        itembs.ApplySockets();
        itemPossesion.Add(itembs, freeslot);
        freeslot.Item = itembs;
        itembs.OnAddedToInventory();
        itembs.Sleep();
        return true;
    }
}
