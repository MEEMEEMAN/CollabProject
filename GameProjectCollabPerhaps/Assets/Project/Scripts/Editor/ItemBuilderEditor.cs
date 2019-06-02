using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Collections;
using System.Text;


[System.Serializable]
public class DisplayHolder
{
    public string identifier;
    public ItemBase displayItem;

    public DisplayHolder()
    {

    }

    public DisplayHolder(string id, ItemBase bs)
    {
        identifier = id;
        displayItem = bs;
    }
}

public class ItemBuilderEditor : EditorWindow
{

    public DisplayHolder[] dispalyItems;
    SerializedObject so;
    SerializedProperty sp1;

    [MenuItem("Window/ItemDatabase")]
    public static void Init()
    {
        //GetWindow<ItemBuilderEditor>("ItemBase Database");
        ItemBuilderEditor window = (ItemBuilderEditor)EditorWindow.GetWindow(typeof(ItemBuilderEditor), true, "ItemBase Database");
        
    }

    void OnEnable()
    {
        so = new SerializedObject(this);
        sp1 = so.FindProperty("dispalyItems");
    }

    Vector2 scrollpos;
    string inputPath = "Assets/Project/Resources/Prefabs/Items";
    string outputPath = "Assets/Project/Resources/JsonData/ItemPathData";

    void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        scrollpos = EditorGUILayout.BeginScrollView(scrollpos, GUILayout.Width(Screen.width/1.5f), GUILayout.Height(100));
        EditorGUILayout.PropertyField(sp1, true);

        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndHorizontal();

        GUILayout.Label("Items path");
        inputPath = EditorGUILayout.TextField(inputPath);
        GUILayout.Label("Json output directory");
        outputPath = EditorGUILayout.TextField(outputPath);

        if (GUILayout.Button("Find and Serialize"))
        {
            Serialize(inputPath);
        }

        so.ApplyModifiedProperties();
    }


    void Serialize(string SearchingStartDirectory)
    {
        ItemBase.ItemPath[] foundItemPaths = findItemPaths(SearchingStartDirectory);
        
        ItemBase.ItemPathSerial ips = new ItemBase.ItemPathSerial();
        ips.data = foundItemPaths;

        string output = CustomTextService.SerializeJson(ips);

        //WriteJson(output, outputPath);
        CustomTextService.WriteJsonToDirectory(output, outputPath, "itempaths");
        AssetDatabase.Refresh();
    }

    List<DisplayHolder> items = new List<DisplayHolder>();
    ItemBase.ItemPath[] findItemPaths(string path)
    {
        List<ItemBase.ItemPath> foundPaths = new List<ItemBase.ItemPath>();
        items.Clear();

        findPathsRecursively(path, ref foundPaths);
        dispalyItems = items.ToArray();
        items.Clear();

        return foundPaths.ToArray();
    }

    void findPathsRecursively(string path, ref List<ItemBase.ItemPath> PathList)
    {
        string[] directories = Directory.GetFileSystemEntries(path);

        foreach (string dirPath in directories)
        {                
            if(dirPath.Contains(".prefab") && !dirPath.Contains(".meta")) //if Directory Path is a prefab
            {
                string localpath = dirPath;
                int index = localpath.IndexOf("/Resources");
                localpath = localpath.Remove(0, index + 11);

                int prefIndex = localpath.IndexOf(".prefab");
                localpath = localpath.Remove(prefIndex, localpath.Length - prefIndex);

                GameObject Loaded = Resources.Load(localpath) as GameObject;

                ItemBase itembs = Loaded.GetComponent<ItemBase>();
                if (itembs != null)
                {
                    ItemBase.ItemPath pc = new ItemBase.ItemPath();
                    pc.itemIdentifier = itembs.identifierName;
                    pc.resourcesPath = localpath;
                    PathList.Add(pc);
                    items.Add(new DisplayHolder(pc.itemIdentifier, itembs));
                }
                else
                {
                    Debug.LogError("ISWACK "+localpath);
                }
                
            }
            else if(!dirPath.Contains(".")) //If path is a folder
            {
                findPathsRecursively(dirPath, ref PathList);
            }
            else
            {
                //Literally do nothing
            }
        }
        
    }
}
