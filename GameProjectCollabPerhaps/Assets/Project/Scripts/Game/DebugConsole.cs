using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ConsoleCommands;

public abstract class ConsoleCommand
{
    public abstract string command { get; protected set; }
    public virtual string description { get; protected set; } = "Lorem blank ipsum description.";
    public virtual int argumentCount { get; protected set; } = 0;

    public void AddCommandToConsole()
    {
        DebugConsole.AddCommandToConsole(command, this);
    }

    public abstract void RunCommand();

}

public class DebugConsole : MonoBehaviour
{
    public static Dictionary<string, ConsoleCommand> CommandList { get; private set; }

    [Header("Console UI")]
    public Canvas consoleCanvas;
    public ScrollRect scrollRect;
    public TextMeshProUGUI consoleText;
    public TMP_InputField inputField;
    public TextMeshProUGUI inputFieldText;
    List<string> argumentHistoryBuffer;

    public static void LogDev(string msg)
    {
        #if UNITY_EDITOR
        Debug.Log(msg);
        Log(msg);
        #else
        Log(msg);
        #endif
    }

    void Awake()
    {
        CommandList = new Dictionary<string, ConsoleCommand>();
    }

    void Start()
    {
        CreateCommands();
        Log("Wack Console ceases to exist unsuccessfully.");
        argumentHistoryBuffer = new List<string>();
        consoleCanvas.gameObject.SetActive(false);
    }

    void CreateCommands()
    {
        QuitCommand quitc = QuitCommand.CreateCommand();
        ItemList listc = ItemList.CreateCommand();
        HelpCommand helpc = HelpCommand.CreateCommand();
        ClearCommand clearc = new ClearCommand();
        Log(string.Format("A total of {0} commands exist.",CommandList.Keys.Count));
        Log("Type 'help' for more info.");
    }

    void LateUpdate()
    {
        if (!consoleCanvas.gameObject.activeInHierarchy)
            return;

        if(consoleText.text.Length > 3000)
        {
            consoleText.text = consoleText.text.Remove(0, 2000);
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            Log(inputFieldText.text, true);
            ParseInput(inputFieldText.text);
            inputField.ActivateInputField();
        }
    }

    public void Clear()
    {
        consoleText.text = "";
    }

    public static void AddCommandToConsole(string name, ConsoleCommand command)
    {
        if (!CommandList.ContainsKey(name))
        {
            name = name.ToLower();
            CommandList.Add(name, command);
        }
        else
        {
            Debug.LogError("Command already Exists");
        }
    }

    void MsgToConsole(string txt, bool userInput = false)
    {
        if(userInput)
        consoleText.text += ">" + txt + "\n";
        else
            consoleText.text += txt + "\n";

        StartCoroutine(WaitForReset());
    }

    IEnumerator WaitForReset()
    {
        yield return new WaitForSeconds(0.01f);
        VerticalScrollToBottom();
    }

    public static void Log(string msg, bool userInput = false)
    {
        GameManager.console.MsgToConsole(msg, userInput);
    }

    public void VerticalScrollToBottom()
    {
        scrollRect.verticalNormalizedPosition = 0f;
        scrollRect.verticalScrollbar.value = 0f;
    }

    void ParseInput(string input)
    {
        string[] args = input.Split(null);

        if (args.Length == 0 || input == null)
        {
            Log("Command to recognized.");
        }

        string command = args[0];

        if(command[command.Length-1] == (char)8203)
        {
            command = command.Remove(command.Length - 1);
        }
        command = command.ToLower();

        if (!CommandList.ContainsKey(command))
        {
            Log(string.Format("'{0}' is not recognized.", args[0]));
        }
        else
        {
            CommandList[command].RunCommand();
        }
    }
}