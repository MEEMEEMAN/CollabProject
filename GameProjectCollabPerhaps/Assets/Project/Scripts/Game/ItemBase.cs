using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

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
    public string identifierName;
    bool instantiated = false;

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
    public static void BuildItemDictionary()
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
        Debug.Log("Item Database was built successfully.");
    }
    
    /// <summary>
    /// The most basic command for getting an item. will return null if there is an error
    /// </summary>
    /// <param name="itemName"></param>
    /// <returns></returns>
    public static ItemBase GetItem(string itemName)
    {
        ItemBase item = null;
        ItemDatabase.TryGetValue(itemName, out item);
        GameObject instance = Instantiate(item.gameObject);
        item = instance.GetComponent<ItemBase>();

        Debug.Log("Creating " + item.identifierName);

        return item;
    }

    public static List<string> GetKeys()
    {
        List<string> keyList = new List<string>(ItemDatabase.Keys);
        return keyList;
    }

    public ItemBase Create(Vector3 pos)
    {
        GameObject instance = Instantiate(gameObject, pos, Quaternion.identity);
        instantiated = true;

        return instance.GetComponent<ItemBase>();
    }

    public bool isInstantiated()
    {
        return instantiated;
    }

    public void DestroyItem()
    {
        Destroy(gameObject);
    }
}
