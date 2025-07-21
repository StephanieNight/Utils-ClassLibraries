using ConsoleUXLibrary.TerminalUX;
using System;

namespace TerminalUX.Menus
{
  public class SingleSelectMenu
  {
    const string CurserOption = "[ ->]";
    const string NonSelectedOption = "[   ]";
    int curserlocation = 0;

    public int StartMenu(string[] options, ConsolePoint point)
    {
      return StartMenu(options, point.x, point.y);
    }
    
    public int StartMenu(string[] options,  int x = -1, int y = -1)
    {     
      if (x == -1 && y == -1)
      {
        var point = TerminalUtils.GetConsolePoint();
        x = point.x;
        y = point.y;
      }

      TerminalUtils.SetCursor(x, y);

      curserlocation = 0;

      bool closeMenu = false;

      while (!closeMenu)
      {
        TerminalUtils.SetCursor(x, y);
        RenderMenu(options);

        // Gets input
        var control = TerminalKeyboard.GetControlInput();
        switch (control)
        {
          case TerminalKeyboard.Up:
            curserlocation--;
            break;
          case TerminalKeyboard.Down:
            curserlocation++;
            break;
          case TerminalKeyboard.EnterInput:
            closeMenu = true;
            break;
          case TerminalKeyboard.Cancel:
            return -1;
        }
        // Clamps the range of curser location to be 0 - options length to awoid selection of options that aint there.
        curserlocation = MenuUtilities.ClampMenuCurser(curserlocation, options.Length);
      }
      return curserlocation;
    }

    private void RenderMenu(string[] options)
    {
      for (int i = 0; i < options.Length; i++)
      {
        // Get prefix of the following order, Is Curser option -> Is Non Selected. 
        var prefix = i == curserlocation ? CurserOption : NonSelectedOption;
        Console.WriteLine(prefix + " : " + options[i]);
      }
    }
  }
}