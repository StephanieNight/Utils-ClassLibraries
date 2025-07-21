using System.Collections.Generic;
using System.Dynamic;

namespace ConsoleUXLibrary.Menus
{
	public class SingleSelectMenu
	{
		const string CurserOption = "[ ->]";
		const string NonSelectedOption = "[   ]";
		int curserlocation = 0;

		public  int StartMenu(string[] options, ConsolePoint point, bool ClearScreen = false)
		{
			return StartMenu(options, "", ClearScreen, point.x, point.y);
		}
		public  int StartMenu(string[] options, int x, int y, bool ClearScreen = false)
		{
			return StartMenu(options, "", ClearScreen, x, y);
		}
		public  int StartMenu(string[] options, string headline = "", bool ClearScreen = false, int x = -1, int y = -1)
		{
			if (ClearScreen) { Terminal.ClearScreen(); }

			if (x == -1 && y == -1)
			{
				var point = Terminal.GetConsolePoint();
				x = point.x; 
				y = point.y;
			}

			Terminal.SetCursor(x, y);

			if (headline != "")
			{
				Terminal.WriteLine(headline);

				y = Terminal.GetLine();

			}

			curserlocation = 0;

			bool closeMenu = false;

			while (!closeMenu)
			{
				Terminal.SetCursor(x, y);
				RenderMenu(options);

				// Gets input
				var control = Terminal.Keyboard.GetControlInput();
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
				Terminal.WriteLine(prefix + " : " + options[i]);
			}
		}
	}
}