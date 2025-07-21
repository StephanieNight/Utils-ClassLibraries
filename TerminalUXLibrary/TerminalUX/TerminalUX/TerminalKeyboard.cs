using System;
using System.Linq;

namespace TerminalUX
{
  public class TerminalKeyboard
  {
    public const ConsoleKey EnterInput = ConsoleKey.Enter;
    public const ConsoleKey Cancel = ConsoleKey.Escape;
    public const ConsoleKey Select = ConsoleKey.Spacebar;
    public const ConsoleKey Up = ConsoleKey.UpArrow;
    public const ConsoleKey Down = ConsoleKey.DownArrow;
    public const ConsoleKey Left = ConsoleKey.LeftArrow;
    public const ConsoleKey Right = ConsoleKey.RightArrow;

    public static ConsoleKey[] ControlKeys = { EnterInput, Cancel, Select, Up, Down, Left, Right };
    public static ConsoleKey[] ArrowKeys = { Up, Down, Left, Right };

    // Utilities
    // ------------------------------------------------------------------------------
    private void ClearKeyBuffer()
    {
      while (Console.KeyAvailable)
      {
        Console.ReadKey(true);
      }
    }
    private bool IsValidControlInput(ConsoleKey key)
    {
      if (ControlKeys.Contains(key)) return true;
      return false;
    }
    private bool IsValidDirectionInput(ConsoleKey key)
    {
      if (ArrowKeys.Contains(key)) return true;
      return false;
    }
    // Get Input
    // ------------------------------------------------------------------------------
    public ConsoleKey GetControlInput()
    {
      ConsoleKey key;
      while (true)
      {
        key = ReadKey();
        if (IsValidControlInput(key)) break;
      }
      return key;
    }
    public ConsoleKey GetDirectionInput()
    {
      ConsoleKey key;
      while (true)
      {
        key = ReadKey();
        if (IsValidDirectionInput(key)) break;
      }
      return key;
    }
    public ConsoleKey ReadKey()
    {
      return Console.ReadKey().Key;
    }
    public string GetProtectedInputString(string header = "password:")
    {
      ClearKeyBuffer();

      if (header != string.Empty) Terminal.WriteLine(header);

      var passphare = "";

      while (true)
      {
        var key = Console.ReadKey(true);
        if (key.Key == EnterInput)
          return passphare;
        if (key.Key == Cancel)
          return "";

        Console.Write("*");

        passphare += key.KeyChar;
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
    public ConsoleCommand? GetCommandWithFlags(string header = "Confirm by pressing [Enter]")
    {
      if (header != "")
      {
        Console.WriteLine(header);
      }
      var Command = Console.ReadLine();

      return new ConsoleCommand(Command);
    }
    // Flow Control
    // ------------------------------------------------------------------------------
    public void InputContinue(string header = "Continue?")
    {
      ClearKeyBuffer();

      if (header != string.Empty) Terminal.WriteLine(header);

      Console.ReadKey();
    }
    public void WaitSpecificInput(string header = "Continue?", ConsoleKey expectedKey = EnterInput)
    {
      ClearKeyBuffer();

      if (header != string.Empty) Terminal.WriteLine(header);

      var wait = true;
      while (wait)
      {
        if (Console.ReadKey().Key == expectedKey) wait = false;
      }
    }
  }
}
