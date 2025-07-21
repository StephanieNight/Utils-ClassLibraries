using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace TerminalUX
{
  public class ConsoleCommand
  {
    private Dictionary<string, string> _flagsWithValues = new Dictionary<string, string>();
    private string[] _flags = new string[0];
    private string? _allFlagsWithValues;
    private string? _command;

    public ConsoleCommand(string? commandWithFlags)
    {
      if (commandWithFlags != null)
      {
        var index = commandWithFlags.IndexOf(" ");
        if (index == -1)
        {
          _command = commandWithFlags;
        }
        else
        {
          _command = commandWithFlags.Substring(0, index);
          _allFlagsWithValues = commandWithFlags.Substring(index + 1);
        }
        SetFlags(_allFlagsWithValues);
      }
    }
    public ConsoleCommand(string command, string allFlagsWithValues)
    {
      _command = command;
      _allFlagsWithValues = allFlagsWithValues;
      SetFlags(_allFlagsWithValues);
    }
    public string? Command { get { return _command; } }
    public Dictionary<string, string> FlagsWithValues { get { return _flagsWithValues; } }
    public string[] Flags { get { return _flags; } }
    private void SetFlags(string? allFlagsWithValues)
    {
      if (allFlagsWithValues == null)
      {
        _flags = new string[0];
        _flagsWithValues = new Dictionary<string, string>();
        return;
      }
      /* Prompt from Chat GPT 3.5 replacing the regex.
       * Here's a breakdown of this regular expression:
       *              
       * (--\w+(?:=(\S+))?|-[\w\d]+): This is the main pattern we're matching against.
       * 
       * --\w+ This part matches long flags that start with -- followed by one or more word characters (\w+). \w+ matches letters, digits, and underscores. This part captures long flags like --output.
       * 
       * (?:=(\S+))? 
       * but ?: at the start makes it non-capturing, meaning we don't capture its content. 
       * This part captures the values associated with flags like --output=output.txt. The ? at the end makes this whole group optional, allowing flags without values like --verbose.
       * 
       * -[\w\d]+: 
       * 
       * In summary, this regular expression allows you to capture both long flags with 
       * optional values (e.g., --output=output.txt) and short flags without values (e.g., -abc). 
       * It separates the flag name and value (if present) into capturing groups, 
       * making it versatile for extracting flags and their associated values from a command-line input string. 
       * 
       */
      string LongFlagwithSentencePattern = @"--(\w+)((\s*=\s*)|\s*)""([^""]+)""";
      string LongFlagPatterh = @"--(\w+)((?:\s(\S+))|(?:=(\S+)))?";
      string shortFlagPattern = @"-(\w+)";


      // Create a dictionary to store the extracted flags and their values
      Dictionary<string, string> flagsWithValues = new Dictionary<string, string>();
      List<string> shortflags = new List<string>();

      // Create a regular expression object
      Regex regex = new Regex(LongFlagwithSentencePattern);
      // Use the regular expression to find matches in the input string
      MatchCollection matches = regex.Matches(allFlagsWithValues);

      // Checks for the Long flags with sentences. By iterating through the matches and add them to the dictionary.
      foreach (Match match in matches)
      {
        string flag = match.Groups[1].Value;
        string value = match.Groups[4].Success ? match.Groups[4].Value : string.Empty;
        flagsWithValues[flag] = value;
      }

      // Checks for the Long flags with single values. By iterating through the matches and add them to the dictionary.
      regex = new Regex(LongFlagPatterh);
      matches = regex.Matches(allFlagsWithValues);
      foreach (Match match in matches)
      {
        string flag = match.Groups[1].Value;
        if (flagsWithValues.Keys.Contains(flag) == false)
        {
          string value = match.Groups[3].Success ? match.Groups[3].Value : match.Groups[4].Success ? match.Groups[4].Value : string.Empty;
          flagsWithValues[flag] = value;
        }
      }

      // Checks for the short flags. By iterating through the matches and add them to the dictionary
      regex = new Regex(shortFlagPattern);
      matches = regex.Matches(allFlagsWithValues);
      foreach (Match match in matches)
      {
        string flag = match.Groups[1].Value;
        if (flagsWithValues.Keys.Contains(flag) == false)
        {
          shortflags.Add(flag);
        }
      }
      _flagsWithValues = flagsWithValues;
      _flags = shortflags.ToArray();
    }
  }
}
