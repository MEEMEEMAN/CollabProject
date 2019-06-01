using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class ItemBsSODisplay
{
    public string displayName;
    public ItemBase item;
}

[CreateAssetMenu(fileName = "Custom", menuName = "ItemBaseScriptableObject")]
public class ItemBaseScripatbleObject : ScriptableObject
{
    public ItemBsSODisplay[] items;
}

[CustomEditor(typeof(ItemBaseScripatbleObject))]
public class ItemBsSOInspector : Editor
{
    ItemBaseScripatbleObject comp;

    void OnEnable()
    {
        comp = (ItemBaseScripatbleObject)target;
    }

    public override void OnInspectorGUI()
    {
        Draw();
        DrawDefaultInspector();
    }

    void Draw()
    {
        if(GUILayout.Button("Build items"))
        {
            ItemBase[] found = Resources.FindObjectsOfTypeAll<ItemBase>();

            ItemBase.BuildItemDictionary(found);

            List<string> keys = ItemBase.GetKeys();
            comp.items = new ItemBsSODisplay[keys.Count];
            Debug.Log("DB Count "+ItemBase.ItemDatabase.Count);

            for (int i = 0; i < keys.Count; i++)
            {
                comp.items[i] = new ItemBsSODisplay();
                comp.items[i].displayName = keys[i];
                comp.items[i].item = ItemBase.GetItem(keys[i]);
            }
        }

        if(GUILayout.Button("Test"))
        {
            Debug.Log("DB Count " + ItemBase.ItemDatabase.Count);
        }
    }
}
