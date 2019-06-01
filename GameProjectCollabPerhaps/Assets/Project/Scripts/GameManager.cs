using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for managing the client.
/// </summary>
public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Signleton pattern for GameManager.
    /// </summary>
    public static GameManager instance;

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
    }
}
