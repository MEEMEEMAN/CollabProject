using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Class for managing the client.
/// </summary>
public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Signleton pattern for GameManager.
    /// </summary>
    public static GameManager instance;
    public TextMeshProUGUI ui;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        ItemBase.BuildItemDictionary();
    }

    void Start()
    {
        // Initialize item database
        
    }

    void OnApplicationQuit()
    {
        //ItemBase.ItemDatabase = new Dictionary<string, ItemBase>();
    }
}
