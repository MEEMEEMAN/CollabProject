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
    public Utillity util;

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
        util = GetComponent<Utillity>();
        uim.DisableConsole();
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


        if(lastUI != currentUI)
        {
            lastUI = currentUI;

            switch (currentUI)
            {
                case UIMODE.PLAY:
                    CustomInputManager.enabled = true;
                    uim.DisableConsole();
                    util.lockCursor = true;
                    break;
                case UIMODE.MENU:
                    break;
                case UIMODE.CONSOLE:
                    CustomInputManager.enabled = false;
                    uim.EnableConsole();
                    util.lockCursor = false;
                    break;
                default:
                    break;
            }

        }
    }

}
