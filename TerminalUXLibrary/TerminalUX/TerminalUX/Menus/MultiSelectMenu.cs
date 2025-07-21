using ConsoleUXLibrary.TerminalUX;
using System;

namespace TerminalUX.Menus
{
  public class MultiSelectMenu
  {
    const string CursorOption = "[ ->]";
    const string SelectedOption = "[ * ]";
    const string NonSelectedOption = "[   ]";
    int cursorLocation = 0;
    public bool[] StartMenu(string[] options, ConsolePoint point)
    {
      return StartMenu(options,point.x, point.y);
    }
    public bool[] StartMenu(string[] options, int x = -1, int y = -1)
    {
      if (x == -1 && y == -1)
      {
        var point = TerminalUtils.GetConsolePoint();
        x = point.x;
        y = point.y;
      }

      TerminalUtils.SetCursor(x, y);

      cursorLocation = 0;

      bool[] selectedItems = new bool[options.Length];

      bool closeMenu = false;

      while (!closeMenu)
      {
        TerminalUtils.SetCursor(x, y);
        RenderMenu(options, selectedItems);

        // Gets input
        var control = TerminalKeyboard.GetControlInput();
        switch (control)
        {
          case TerminalKeyboard.Up:
            cursorLocation--;
            break;
          case TerminalKeyboard.Down:
            cursorLocation++;
            break;
          case TerminalKeyboard.Select:
            selectedItems[cursorLocation] = selectedItems[cursorLocation] ? false : true;
            break;
          case TerminalKeyboard.EnterInput:
            closeMenu = true;
            break;
          case TerminalKeyboard.Cancel:
            return new bool[options.Length];
        }
        // Clamps the range of cursor location to be 0 - options length to avoid selection of options that ain't there.
        cursorLocation = MenuUtilities.ClampMenuCurser(cursorLocation, options.Length);
      }
      return selectedItems;
    }

    private void RenderMenu(string[] options, bool[] selectedItems)
    {
      for (int i = 0; i < options.Length; i++)
      {
        // Get prefix of the following order, Is Cursor option -> Is Selected option -> Is Non Selected. 
        var prefix = i == cursorLocation ? CursorOption : selectedItems[i] ? SelectedOption : NonSelectedOption;
        Console.WriteLine(prefix + " : " + options[i]);
      }
    }
  }
}