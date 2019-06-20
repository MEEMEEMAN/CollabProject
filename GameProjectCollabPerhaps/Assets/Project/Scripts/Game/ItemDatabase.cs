using System.Collections.Generic;
using UnityEngine;
using System;

public static class ItemDatabase
{
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
    private static Dictionary<string, ItemBase> ItemList = new Dictionary<string, ItemBase>();

    /// <summary>
    /// Function used by the GameManager to initialize the client's item database.
    /// </summary>
    static void LoadItemDictionary()
    {
        ItemPathSerial itemDataCollection = new ItemPathSerial();
        try
        {
            string itemPathJson = CustomTextService.ReadJsonFromResources("JsonData/ItemPathData/itempaths"); //Take all of the item's filepath json file and convert to string
            itemDataCollection = CustomTextService.DeSerializeJson<ItemPathSerial>(itemPathJson); //DeSerialize the json
        }
        catch(Exception e)
        {
            Debug.LogError(e.ToString());
        }

        ItemList = new Dictionary<string, ItemBase>(); // Initialize the ItemDatabase Dictionary
        foreach (ItemPath item in itemDataCollection.data)
        {
            ItemBase itembs = (Resources.Load(item.resourcesPath) as GameObject).GetComponent<ItemBase>(); //as simple as it gets
            ItemList.Add(item.itemIdentifier, itembs);
        }
        DebugConsole.LogDev(string.Format("Item Database was succesfully loaded with {0} total items.", ItemList.Count));
    }

    public static List<string> GetExistingIdentifiers()
    {
        List<string> keyList = new List<string>(ItemList.Keys);
        return keyList;
    }

    public static void InitializeDatabase()
    {
        LoadItemDictionary();
    }

    /// <summary>
    /// Returns the item if found. if not, returns null.
    /// </summary>
    /// <param name="itemIdentifier"></param>
    /// <returns></returns>
    public static ItemBase Fetch(string itemIdentifier)
    {
        ItemBase item = null;
        ItemList.TryGetValue(itemIdentifier, out item);
        return item;
    }
}
