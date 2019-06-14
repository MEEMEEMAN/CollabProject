using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for managing the client.
/// </summary>
public class GameManager : MonoBehaviour
{
    public enum UIMODE
    {
        PLAY = 0, MENU, CONSOLE
    }
    
    /// <summary>
    /// Signleton pattern for GameManager.
    /// </summary>
    public static GameManager instance;
    public static DebugConsole console;
    public static GamePlayer localPlayer;
    [SerializeField] UIManager uim;
    UIMODE currentUI = UIMODE.PLAY;
    public SettingsManager util;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            console = GetComponentInChildren<DebugConsole>();
            ItemBase.LoadItemDictionary();
            QualitySettings.vSyncCount = 1;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        util = GetComponent<SettingsManager>();
        uim.DisableConsole();
        uim.DisableInventory();
    }

    void Update()
    {
        UIInputListener();
        uim.fpsCounter.text = string.Format("FPS: {0:0}", 1 / Time.deltaTime);
    }

    public UIManager GetUIManager()
    {
        return uim;
    }

    UIMODE lastUI;
    void UIInputListener()
    {
        if(Input.GetKeyDown(KeyCode.F1))
        {
            if (currentUI == UIMODE.PLAY)
                currentUI = UIMODE.CONSOLE;
            else
                currentUI = UIMODE.PLAY;
        }
        else if(Input.GetKeyDown(KeyCode.I))
        {
            if(currentUI == UIMODE.PLAY)
            {
                currentUI = UIMODE.MENU;
            }
            else
            {
                currentUI = UIMODE.PLAY;
            }
        }


        if(lastUI != currentUI) //Functions are runned once
        {
            lastUI = currentUI;

            uim.DisableConsole();
            uim.DisableInventory();
            util.lockCursor = false;
            CustomInputManager.enabled = true;

            switch (currentUI)
            {
                case UIMODE.PLAY:
                    uim.DisableConsole();
                    util.lockCursor = true;
                    break;
                case UIMODE.MENU:
                    uim.EnableInventory();
                    CustomInputManager.enabled = false;
                    break;
                case UIMODE.CONSOLE:
                    CustomInputManager.enabled = false;
                    uim.EnableConsole();
                    break;
                default:
                    break;
            }

        }
    }

    public GamePlayer getLocalPlayer()
    {
        return localPlayer;
    }
}
