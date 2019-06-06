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
    [SerializeField] UIManager uim;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            ItemBase.BuildItemDictionary();
            QualitySettings.vSyncCount = 1;
        }
        else
        {
            Destroy(gameObject);
        }

    }

    void Update()
    {
        uim.fpsCounter.text = string.Format("FPS: {0:0}", 1 / Time.deltaTime);
    }

    public UIManager GetUIManager()
    {
        return uim;
    }

}
