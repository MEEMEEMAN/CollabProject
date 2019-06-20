using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ItemImportData
{
    public string nameIdentifier;
    public string resourcePath;
}

/// <summary>
/// Base class for all of the game's items.
/// </summary>
public class ItemBase : MonoBehaviour
{
    /// <summary>
    /// All Items have an ID name. used for getting the said item out of the database.
    /// </summary>
    [SerializeField]string identifierName;

    /// <summary>
    // The player that posseses this item, null if it's just laying around.
    /// </summary>
    [SerializeField]protected GamePlayer Owner;

    bool created = false;
    public Transform colliders;
    public Sprite InventoryImage;

    public void SetOwner(GamePlayer player)
    {
        Owner = player;
    }

    public GamePlayer GetOnwer()
    {
        return Owner;
    }

    /// <summary>
    /// Get and instantiate the requested item.
    /// </summary>
    /// <param name="identifier"></param>
    /// <returns></returns>
    public static ItemBase Create(string identifier)
    {
        return ItemDatabase.Fetch(identifier).Create();
    }

    /// <summary>
    /// Fetch item reference from the database. ONLY REFERENCE, NO INSTANTIATION
    /// </summary>
    /// <param name="itemName"></param>
    /// <returns></returns>
    public static ItemBase GetItem(string itemName)
    {
        return ItemDatabase.Fetch(itemName);
    }

    /// <summary>
    /// Create, spawns it in the 0,0,0 of the world coordinates.
    /// </summary>
    /// <returns></returns>
    ItemBase InstantiateItem()
    {
        if (created)
            return this;

        GameObject instance = Instantiate(gameObject, Vector3.zero, Quaternion.identity);
        ItemBase itemInstance = instance.GetComponent<ItemBase>();
        itemInstance.created = true;
        itemInstance.transform.localScale = new Vector3(1, 1, 1);
        itemInstance.gameObject.SetActive(true);

        return itemInstance;
    }

    public string GetIdentifier()
    {
        return identifierName;
    }

    public ItemBase Create()
    {
        return InstantiateItem();
    }

    public virtual void ApplySockets()
    {
        transform.SetParent(Owner.GetInventory().getItemParent());
    }

    public ItemBase Create(Vector3 pos)
    {
        ItemBase item = InstantiateItem();
        item.transform.position = pos;
        return item;
    }

    public bool isCreated()
    {
        return created;
    }

    public void DestroyItem()
    {
        Destroy(gameObject);
    }

    public virtual void Sleep()
    {
        gameObject.SetActive(false);
    }
    
    public virtual void WakeUp()
    {
        gameObject.SetActive(true);
    }

    public bool isSleeping()
    {
        return transform.gameObject.activeInHierarchy;
    }

    public virtual void OnAddedToInventory()
    {
        transform.position = Owner.transform.position;
    }
}
