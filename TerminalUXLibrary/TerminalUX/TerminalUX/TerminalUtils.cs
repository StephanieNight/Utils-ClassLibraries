using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerminalUX;

namespace ConsoleUXLibrary.TerminalUX
{
  static class TerminalUtils
  {
    // UX Utilities
    // ----------------------------------------------------------------------------------

    public static ConsolePoint GetConsolePoint()
    {
      ConsolePoint point = new ConsolePoint()
      {
        x = Console.CursorLeft,
        y = Console.CursorTop,
      };
      return point;
    }
    public static int GetLine()
    {
      return Console.CursorTop;
    }
    public static void SetCursor(ConsolePoint point)
    {
      SetCursor(point.x, point.y);
    }
    public static void SetCursor(int x = 0, int y = 0)
    {
      Console.SetCursorPosition(x, y);
    }
    // UX Table 
    // ----------------------------------------------------------------------------------
    public static void WriteTableBorder(int[] columnWidths)
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
    public static void WriteTableRow(int[] collumnWidths, string[] data)
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
    public static void WriteTableHeader(int[] columnWidths, string[] headers)
    {
      WriteTableBorder(columnWidths);
      WriteTableRow(columnWidths, headers);
      WriteTableBorder(columnWidths);
    }
    public static int[] GetColumnWidts(string[] headers, string[][] data)
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
