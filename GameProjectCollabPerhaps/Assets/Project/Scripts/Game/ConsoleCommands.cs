using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ConsoleCommands
{

    public class QuitCommand : ConsoleCommand
    {
        public override string command { get; protected set; }

        public QuitCommand()
        {
            command = "quit";
            description = "Quits the game.";
            AddCommandToConsole();
        }

        public override void RunCommand()
        {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
        }

        public static QuitCommand CreateCommand()
        {
            return new QuitCommand();
        }

    }

    public class ItemList : ConsoleCommand
    {
        public override string command { get; protected set; }

        public ItemList()
        {
            command = "itemlist";
            description = "Displays the game's entire item list.";
            AddCommandToConsole();
        }

        public override void RunCommand()
        {
            List<string> keys = new List<string>(ItemBase.ItemDatabase.Keys);
            string[] keyarray = keys.ToArray();
            DebugConsole.Log("The items are as follows:");
            for (int i = 0; i < keyarray.Length; i++)
            {
                DebugConsole.Log(keyarray[i]);
            }
            DebugConsole.Log(string.Format("a total of {0} items.", keyarray.Length));
        }

        public static ItemList CreateCommand()
        {
            return new ItemList();
        }
    }

    public class HelpCommand : ConsoleCommand
    {
        public override string command { get; protected set; }

        public HelpCommand()
        {
            command = "help";
            description = "Shows all available commands.";
            AddCommandToConsole();
        }

        public static HelpCommand CreateCommand()
        {
            return new HelpCommand();
        }

        public override void RunCommand()
        {
            List<string> commands = new List<string>(DebugConsole.CommandList.Keys);
            string[] comms = commands.ToArray();
            for (int i = 0; i < DebugConsole.CommandList.Count; i++)
            {
                DebugConsole.Log(string.Format("{0} - {1}", comms[i], DebugConsole.CommandList[comms[i]].description));
            }
        }
    }

    public class ClearCommand : ConsoleCommand
    {
        public override string command { get; protected set; }

        public ClearCommand()
        {
            command = "clear";
            description = "Clears the console.";
            AddCommandToConsole();
        }

        public override void RunCommand()
        {
            GameManager.console.Clear();
        }
    }

}
