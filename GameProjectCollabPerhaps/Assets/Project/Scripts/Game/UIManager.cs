using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI fpsCounter;
    public Canvas consoleCanvas;
    public Canvas inventoryCanvas;
    [SerializeField] Sprite defaultIitemImage;

   public void EnableConsole()
   {
        consoleCanvas.gameObject.SetActive(true);
   }

    public void DisableConsole()
    {
        consoleCanvas.gameObject.SetActive(false);
    }

    public void EnableInventory()
    {
        inventoryCanvas.gameObject.SetActive(true);
    }

    public void DisableInventory()
    {
        inventoryCanvas.gameObject.SetActive(false);
    }

    public Sprite getDefaultSprite()
    {
        return defaultIitemImage;
    }
}
