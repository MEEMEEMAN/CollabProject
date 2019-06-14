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
    [SerializeField] string identifierName;

    bool created = false;
    public Transform colliders;
    public Sprite itemImage;

    [System.Serializable]
    public struct ItemPath
    {
        public string itemIdentifier;
        public string resourcesPath;
    }

    [System.Serializable]
    public struct ItemPathSerial
    {
        public ItemPath[] data;
    }

    /// <summary>
    /// Main item database.
    /// </summary>
    public static Dictionary<string, ItemBase> ItemDatabase = new Dictionary<string, ItemBase>();

    /// <summary>
    /// Function used by the GameManager to initialize the client's item database.
    /// </summary>
    public static void LoadItemDictionary()
    {
        ItemPathSerial itemDataCollection = new ItemPathSerial();
        try
        {
            string itemPathJson = CustomTextService.ReadJsonFromResources("JsonData/ItemPathData/itempaths"); //Take all of the item's filepath json file and convert to string
            itemDataCollection = CustomTextService.DeSerializeJson<ItemPathSerial>(itemPathJson); //DeSerialize the json
        }
        catch
        {
            Debug.LogError("File doesnt exist?");
        }

        ItemDatabase = new Dictionary<string, ItemBase>(); // Initialize the ItemDatabase Dictionary
        foreach (ItemPath item in itemDataCollection.data)
        {
            ItemBase itembs = (Resources.Load(item.resourcesPath) as GameObject).GetComponent<ItemBase>(); //as simple as it gets
            ItemDatabase.Add(item.itemIdentifier, itembs);
        }
        DebugConsole.LogDev(string.Format("Item Database was succesfully loaded with {0} total items.", ItemDatabase.Count));
    }
    
    /// <summary>
    /// Fetch item reference from the database. ONLY REFERENCE, NO INSTANTIATION
    /// </summary>
    /// <param name="itemName"></param>
    /// <returns></returns>
    public static ItemBase GetItem(string itemName)
    {
        ItemBase item = null;
        ItemDatabase.TryGetValue(itemName, out item);
        return item;
    }

    /// <summary>
    /// Create, spawns it in the 0,0,0 of the world coordinates.
    /// </summary>
    /// <returns></returns>
    ItemBase InstantiateItem()
    {
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

    public ItemBase Create(Vector3 pos)
    {
        ItemBase item = InstantiateItem();
        item.transform.position = pos;
        return item;
    }

    public static List<string> GetKeys()
    {
        List<string> keyList = new List<string>(ItemDatabase.Keys);
        return keyList;
    }

    public bool isCreated()
    {
        return created;
    }
    
    public void DestroyItem()
    {
        Destroy(gameObject);
    }

    public void Sleep()
    {
        transform.root.gameObject.SetActive(false);
    }
    
    public void WakeUp()
    {
        transform.root.gameObject.SetActive(true);
    }

    public bool isSleeping()
    {
        return transform.root.gameObject.activeInHierarchy;
    }
}
