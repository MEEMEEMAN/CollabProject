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

    public static void BuildItemDictionary()
    {
        ItemPathSerial itemDataCollection = new ItemPathSerial();
        try
        {
            string itemPathJson = CustomTextService.ReadJsonFromResources("JsonData/ItemPathData/itempaths");
            itemDataCollection = CustomTextService.DeSerializeJson<ItemPathSerial>(itemPathJson);
        }
        catch
        {
            Debug.LogError("File doesnt exist?");
        }

        ItemDatabase = new Dictionary<string, ItemBase>();
        foreach (ItemPath item in itemDataCollection.data)
        {
            ItemBase i = (Resources.Load(item.resourcesPath) as GameObject).GetComponent<ItemBase>();
            ItemDatabase.Add(item.itemIdentifier, i);
        }
        Debug.Log("Item Database was built successfully.");
    }
    
    public static ItemBase GetItem(string itemName)
    {
        ItemBase item;
        ItemDatabase.TryGetValue(itemName, out item);
        GameObject instance = Instantiate(item.gameObject);
        Debug.Log(instance.transform.position);

        return instance.GetComponent<ItemBase>();
    }

    public static List<string> GetKeys()
    {
        List<string> keyList = new List<string>(ItemDatabase.Keys);
        return keyList;
    }

    public ItemBase Create(Vector3 pos)
    {
        GameObject instance = Instantiate(gameObject, pos, Quaternion.identity);

        return instance.GetComponent<ItemBase>();
    }


    public string identifierName;
}
