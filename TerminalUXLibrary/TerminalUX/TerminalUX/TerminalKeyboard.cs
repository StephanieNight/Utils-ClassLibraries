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
    public static void ClearKeyBuffer()
    {
      while (Console.KeyAvailable)
      {
        Console.ReadKey(true);
      }
    }
    public static bool IsValidControlInput(ConsoleKey key)
    {
      if (ControlKeys.Contains(key)) return true;
      return false;
    }
    public static bool IsValidDirectionInput(ConsoleKey key)
    {
      if (ArrowKeys.Contains(key)) return true;
      return false;
    }
    // Get Input
    // ------------------------------------------------------------------------------
    public static ConsoleKey GetControlInput()
    {
      ConsoleKey key;
      while (true)
      {
        key = ReadKey();
        if (IsValidControlInput(key)) break;
      }
      return key;
    }
    public static ConsoleKey GetDirectionInput()
    {
      ConsoleKey key;
      while (true)
      {
        key = ReadKey();
        if (IsValidDirectionInput(key)) break;
      }
      return key;
    }
    public static ConsoleKey ReadKey()
    {
      return Console.ReadKey().Key;
    }   
  }
}
