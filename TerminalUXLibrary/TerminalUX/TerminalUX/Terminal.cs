using System;
using System.Collections.Generic;
using System.Linq;
using TerminalUX.Menus;

namespace TerminalUX
{
  public class Terminal
  {
    private TerminalKeyboard _keyboard = new TerminalKeyboard();
    private MultiSelectMenu _multiSelectMenu = new MultiSelectMenu();
    private SingleSelectMenu _singleSelectMenu = new SingleSelectMenu();

    private int _screenWidth = 35;
    private string _seperationLine = "";
    private string _doubleSeperationLine = "";

    public TerminalKeyboard Keyboard { get { return _keyboard; } }
    public MultiSelectMenu MultiSelectMenu { get { return _multiSelectMenu; } }
    public SingleSelectMenu SingleSelectMenu { get { return _singleSelectMenu; } }


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
        for (int i = _seperationLine.Length; i < _screenWidth; i++)
        {
          _seperationLine += "-";
        }
      }
      WriteLine(_seperationLine);
      return _seperationLine;
    }
    public string DoubleSeparatorLine()
    {
      if (_doubleSeperationLine.Length < _screenWidth)
      {
        for (int i = _doubleSeperationLine.Length; i < _screenWidth; i++)
        {
          _doubleSeperationLine += "=";
        }
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
    public string WriteLine(string line)
    {
      SetScreenWidth(line.Length);
      Console.WriteLine(line);
      return line;
    }
    // UX Utilities
    // ----------------------------------------------------------------------------------
    public void ClearScreen()
    {
      Console.Clear();
    }
    public ConsolePoint GetConsolePoint()
    {
      ConsolePoint point = new ConsolePoint()
      {
        x = Console.CursorLeft,
        y = Console.CursorTop,
      };
      return point;
    }
    public int GetLine()
    {
      return Console.CursorTop;
    }
    public void SetCursor(ConsolePoint point)
    {
      SetCursor(point.x, point.y);
    }
    public void SetCursor(int x = 0, int y = 0)
    {
      Console.SetCursorPosition(x, y);
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

      var columnwidth = GetColumnWidts(headers, data);
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
      var columnWidths = GetColumnWidts(headers, data);

      if (headers.Length != 0)
      {
        WriteTableHeader(columnWidths, headers);
      }
      foreach (var row in data)
      {
        WriteTableRow(columnWidths, row);
      }
      WriteTableBorder(columnWidths);
    }
    private void WriteTableBorder(int[] columnWidths)
    {
      foreach (var width in columnWidths)
      {
        Console.Write("+");
        for (int i = 0; i < width; i++)
        {
          Console.Write("-");
        }
      }
      Console.WriteLine("+");
    }
    private void WriteTableRow(int[] collumnWidths, string[] data)
    {
      if (collumnWidths.Length != data.Length)
      {
        throw new ArgumentException("Columns does not equal data");
      }
      for (int i = 0; i < collumnWidths.Length; i++)
      {
        Console.Write("|");
        Console.Write(data[i]);
        for (int d = data[i].Length; d < collumnWidths[i]; d++)
        {
          Console.Write(" ");
        }
      }
      Console.WriteLine("|");
    }
    private void WriteTableHeader(int[] columnWidths, string[] headers)
    {
      WriteTableBorder(columnWidths);
      WriteTableRow(columnWidths, headers);
      WriteTableBorder(columnWidths);
    }
    private static int[] GetColumnWidts(string[] headers, string[][] data)
    {
      var collumnsWidthslist = new List<int>();
      foreach (var i in headers)
      {
        collumnsWidthslist.Add(i.Length);
      }
      var collumnWidths = collumnsWidthslist.ToArray();
      foreach (var row in data)
      {
        for (int i = 0; i < row.Length; i++)
        {
          if (collumnWidths[i] < row[i].Length)
          {
            collumnWidths[i] = row[i].Length;
          }

        }
      }
      return collumnWidths.ToArray();
    }
  }
}

