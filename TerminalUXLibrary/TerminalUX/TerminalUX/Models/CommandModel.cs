using System;
using System.Collections.Generic;
using System.Text;

namespace TerminalUX.Models
{
  public class CommandModel
  {
    public string Name { get; }
    public string CommandDescription { get; private set; }
    public Dictionary<string, Action<string[]>> Flags { get; }
    public Dictionary<string, string> Tags { get; }
    public Dictionary<string, string> FlagDescriptions { get; }
    public Action? DefaultAction { get; private set; } // Default action if no flag is provided
    public CommandModel(string name)
    {
      Name = name;
      Flags = new Dictionary<string, Action<string[]>>();
      FlagDescriptions = new Dictionary<string, string>();
      Tags = new Dictionary<string, string>();
    }
    // Public 
    // ----------------------------------------------------------------------------
    public void AddFlag(string flag, Action action, string description = "")
    {
      AddFlag(flag, _ => action(), description);
    }
    public void AddFlag(string flag, Action<string[]> action, string description = "")
    {
      Flags[flag] = action;
      if (!string.IsNullOrWhiteSpace(description))
      {
        FlagDescriptions[flag] = description;
      }
    }

    public void GenerateTagsForFlags()
    {
      foreach (string flag in Flags.Keys)
      {
        int startIndex = 0;
        if (Flags.Count > 1)
        {
          if (!FindStartIndexForTag(flag, out startIndex))
          {
            continue;
          }
        }
        AddTagForFlag(flag, startIndex);
      }
    }

    public void SetCommandDefaultAction(Action action)
    {
      DefaultAction = action;
    }
    public void SetCommandDescription(string description)
    {
      CommandDescription = description;
    }

    public string GetHelp()
    {
      StringBuilder helpText = new StringBuilder();
      helpText.AppendLine($"{Name} : ");
      helpText.AppendLine($"Decription : {CommandDescription}");
      foreach (var flag in Flags.Keys)
      {
        helpText.Append($"--{flag} ");
        foreach (var tagPair in Tags)
        {
          if (tagPair.Value == flag)
          {
            helpText.Append($"-{tagPair.Key} ");
          }
        }

        if (FlagDescriptions.TryGetValue(flag, out string desc))
        {
          helpText.Append($": {desc}");
        }
        helpText.AppendLine("");
      }
      return helpText.ToString();
    }

    public bool Invoke(string lookup, string[] flagArgs)
    {
      string flag = "";
      if (lookup.Contains("--"))
      {
        flag = lookup[2..];
      }
      else if (lookup.Contains("-"))
      {
        var tag = lookup[1..];
        if (!Tags.TryGetValue(tag, out flag))
        {
          return false;
        }
      }
      if (!Flags.TryGetValue(flag, out Action<string[]> action))
      {
        return false;
      }
      action.Invoke(flagArgs);
      return true;
    }
    // Privates 
    // ----------------------------------------------------------------------------
    private bool FindStartIndexForTag(string flag, out int startIndex)
    {
      string dublicate = "";
      for (int i = 0; i < flag.Length; i++)
      {
        dublicate += flag[i];
        foreach (string otherFlag in Flags.Keys)
        {
          if (otherFlag == flag)
          {
            continue;
          }
          if (otherFlag.StartsWith(dublicate))
          {
            break;
          }
          else
          {
            startIndex = i;
            return true;
          }
        }
      }
      startIndex = -1;
      return false;
    }
    private void AddTagForFlag(string flag, int startIndex)
    {
      int index = startIndex;
      while (index < flag.Length)
      {
        string potentialTag = $"{flag[index]}";
        if (!Tags.ContainsKey(potentialTag))
        {
          Tags.Add(potentialTag, flag);
          return;
        }
        index++;
      }
    }
  }
}
