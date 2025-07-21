using ConsoleUXLibrary.TerminalUX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Channels;
using TerminalUX.Menus;
using TerminalUX.Models;

namespace TerminalUX
{
  public class Terminal
  {
    private TerminalKeyboard _keyboard = new TerminalKeyboard();
    private MultiSelectMenu _multiSelectMenu = new MultiSelectMenu();
    private SingleSelectMenu _singleSelectMenu = new SingleSelectMenu();

    Dictionary<string, CommandModel> Commands = new Dictionary<string, CommandModel>();

    private int _screenWidth = 35;
    private string _seperationLine = "";
    private string _doubleSeperationLine = "";

    public MultiSelectMenu MultiSelectMenu { get { return _multiSelectMenu; } }
    public SingleSelectMenu SingleSelectMenu { get { return _singleSelectMenu; } }

    public Terminal()
    {
      var command = new CommandModel("help");
      command.SetCommandDescription("Prints all The usage messages of every registered command");
      command.SetCommandDefaultAction(HelpCommand);
      AddCommand(command);
    }
    private void UpdateSeperatorSize()
    {
      if(_screenWidth <= 5)
      {
        for (int i = _seperationLine.Length; i < _screenWidth; i++)
        {
          _seperationLine += "-";
        }
        return;
      }
      var side_size = Math.Ceiling((_screenWidth - 5) / 2.0);
      _seperationLine = "";
      for (int i = 0; i < side_size; i++)
      {
        _seperationLine += "-";
      }
      _seperationLine += " *** ";
      for (int i = 0; i < side_size; i++)
      {
        _seperationLine += "-";
      }
    }
    private void UpdateDoubleSeperatorSize()
    {
      for (int i = _doubleSeperationLine.Length; i < _screenWidth; i++)
      {
        _doubleSeperationLine += "=";
      }
    }

    public void AddCommand(CommandModel command)
    {
      Commands.Add(command.Name, command);
    }
    public void ExecuteCommand(string[] parts)
    {
      string commandName = parts[0].ToLower();

      if (!Commands.TryGetValue(commandName, out CommandModel command))
      {
        Console.WriteLine($"Unknown command: {commandName}");
        return;
      }

      if (parts.Length == 1)
      {
        if (command.DefaultAction != null)
        {
          command.DefaultAction?.Invoke(); // Run default action if available
          return;
        }
        WriteLine(command.GetHelp());
        WaitForKeypress();
        return;
      }

      string action = parts[1].ToLower();
      string[] flagArgs = parts.Length > 2 ? parts[2..] : new string[0];
      if (!command.Invoke(action, flagArgs))
      {
        WriteLine($"Cant Invoke action {action}, Incorect use of command {command.Name}");
      }
    }
    public void HelpCommand()
    {
      foreach (var command in Commands.Values)
      {
        if (command.Name == "help")
        {
          continue;
        }
        WriteLine(command.GetHelp());
      }
      WaitForKeypress();
    }
    private void ClearInputBuffer()
    {
      while (Console.KeyAvailable)
        Console.ReadKey(false); // skips previous input chars
    }
    public void WaitForKeypress()
    {
      ClearInputBuffer();
      Console.ReadKey();
    }

    public string[] ParseCommand(string fullstring)
    {
      List<string> commands = new List<string>();
      string current = "";
      bool isParameter = false;
      foreach (char c in fullstring)
      {
        // Check for a split and add the command to the new 
        if (c == ' ' && isParameter == false)
        {
          commands.Add(current);
          current = "";
          continue;
        }
        if (c == '"')
        {
          // toggle parameter
          isParameter = !isParameter;
          continue;
        }
        current += c;
      }
      commands.Add(current);
      if (commands.Count == 1)
      {
        if (commands[0] == "")
        {
          return new string[0];
        }
      }
      for (int i = 0; i < commands.Count; i++)
      {
        var command = commands[i];
        command = command.ToLower();
        commands[i] = command;
      }
      return commands.ToArray();
    }
    // Common UX Phrases
    // ----------------------------------------------------------------------------------
    public void SetScreenWidth(int screenWidth)
    {
      if (screenWidth > _screenWidth)
      {
        _screenWidth = screenWidth;
      }
    }
    public string SeparatorLine(int screenWidth)
    {
      SetScreenWidth(screenWidth);
      return SeparatorLine();
    }
    public string DoubleSeparatorLine(int screenWidth)
    {
      SetScreenWidth(screenWidth);
      return DoubleSeparatorLine();
    }
    public string SeparatorLine()
    {
      if (_seperationLine.Length < _screenWidth)
      {
        UpdateSeperatorSize();
      }
      WriteLine(_seperationLine);
      return _seperationLine;
    }
    public string DoubleSeparatorLine()
    {
      if (_doubleSeperationLine.Length < _screenWidth)
      {
        UpdateDoubleSeperatorSize();
      }
      WriteLine(_doubleSeperationLine);
      return _doubleSeperationLine;
    }
    public string WriteBlankLine()
    {
      WriteLine("");
      return "";
    }
    public string WriteContinue()
    {
      string line = "Continue?..";
      WriteLine(line);
      return line;
    }
    public string WriteCompleted()
    {
      string line = "Completed...";
      WriteLine(line);
      return line;
    }
    public string WriteReady()
    {
      string line = "Ready...";
      WriteLine(line);
      return line;
    }
    public void Write(string value)
    {
      Console.Write(value);
    }
    public string WriteLine(string line = "")
    {
      SetScreenWidth(line.Length);
      Console.WriteLine(line);
      return line;
    }

    public void ClearScreen()
    {
      Console.Clear();
    }

    // Flow Control
    // ------------------------------------------------------------------------------
    public string InputProtected(string header = "password:")
    {
      TerminalKeyboard.ClearKeyBuffer();

      if (header != string.Empty) WriteLine(header);

      var passphare = "";

      while (true)
      {
        var key = Console.ReadKey(true);
        if (key.Key == TerminalKeyboard.EnterInput)
          return passphare;
        if (key.Key == TerminalKeyboard.Cancel)
          return "";

        Console.Write("*");

        passphare += key.KeyChar;
      }
    }
    public void InputContinue(string header = "Continue?")
    {
      TerminalKeyboard.ClearKeyBuffer();

      if (header != string.Empty)
      {
        WriteLine(header);
      }
      Console.ReadKey();
    }
    public string Input()
    {
      return Console.ReadLine();
    }

    public void WaitSpecificInput(string header = "Continue?", ConsoleKey expectedKey = TerminalKeyboard.EnterInput)
    {
      TerminalKeyboard.ClearKeyBuffer();

      if (header != string.Empty) WriteLine(header);

      var wait = true;
      while (wait)
      {
        if (Console.ReadKey().Key == expectedKey) wait = false;
      }
    }
    public string? Prompt(string header = "Confirm by pressing [Enter]")
    {
      if (header != "")
      {
        Console.WriteLine(header);
      }
      return Console.ReadLine();
    }

    // Tables
    // ----------------------------------------------------------------------------------
    public ConsolePoint GetTableSize(string[] headers, string[][] data)
    {
      var width = 0;
      var height = 0;

      if (headers.Length > 0)
      {
        height = 3; // Header.
      }
      foreach (var row in data)
      {
        height += 1;
      }
      height += 1;

      var columnwidth = TerminalUtils.GetColumnWidts(headers, data);
      width += 1;
      foreach (var column in columnwidth)
      {
        width += column + 1;
      }
      return new ConsolePoint(width, height);
    }
    public void CreateTable(string[] headers, string[][] data)
    {
      if (data.Length != 0)
      {
        if (headers.Length != data[0].Length)
        {
          throw new ArgumentException("there is not an equal number of data columns as headers.");
        }
      }
      var columnWidths = TerminalUtils.GetColumnWidts(headers, data);

      if (headers.Length != 0)
      {
        TerminalUtils.WriteTableHeader(columnWidths, headers);
      }
      foreach (var row in data)
      {
        TerminalUtils.WriteTableRow(columnWidths, row);
      }
      TerminalUtils.WriteTableBorder(columnWidths);
    }
  }
}

