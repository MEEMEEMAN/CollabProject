using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI fpsCounter;
    public Canvas consoleCanvas;

   public void EnableConsole()
   {
        consoleCanvas.gameObject.SetActive(true);
   }

    public void DisableConsole()
    {
        consoleCanvas.gameObject.SetActive(false);
    }
}
