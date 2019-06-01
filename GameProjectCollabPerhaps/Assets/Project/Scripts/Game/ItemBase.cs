using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Base class for all of the game's items.
/// </summary>
public class ItemBase : MonoBehaviour
{
    /// <summary>
    /// Main item database.
    /// </summary>
    public static Dictionary<string, ItemBase> ItemDatabase = new Dictionary<string, ItemBase>();

    /// <summary>
    /// Identifies whether found item is a scene item or an asset item.
    /// we need to only add asset gameobject items to the list.
    /// this function ensures only asset gameobjects get added to the ItemDatabase Dictionary.
    /// </summary>
    /// <param name="items"></param>
    public static void BuildItemDictionary(ItemBase[] items)
    {
        ItemDatabase.Clear();

        for (int i = 0; i < items.Length; i++)
        {
            ItemBase item = items[i];
            if (AssetDatabase.GetAssetPath(item.GetInstanceID()) == "")
            {
                Debug.Log(items[i].transform.name + " is a scene item.");
            }
            else
            {
                ItemDatabase.Add(item.identifierName, item);
                Debug.Log("Added " + item.identifierName + " To the item database.");
            }
        }
    }

    public static ItemBase GetItem(string itemName)
    {
        ItemBase item;
        ItemDatabase.TryGetValue(itemName, out item);
        return item;
    }

    public static List<string> GetKeys()
    {
        List<string> keyList = new List<string>(ItemDatabase.Keys);
        return keyList;
    }

    public ItemBase CreateItem(Vector3 pos)
    {
        GameObject instance = Instantiate(gameObject, pos, Quaternion.identity);

        return instance.GetComponent<ItemBase>();
    }


    public string identifierName;
}
