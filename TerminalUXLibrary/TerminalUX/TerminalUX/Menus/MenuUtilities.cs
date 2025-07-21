namespace TerminalUX.Menus
{
  public class MenuUtilities
  {
    internal static int ClampMenuCurser(int curserlocation, int optionsLenght)
    {
      return curserlocation < 0 ? optionsLenght - 1 : curserlocation % optionsLenght;
    }
  }
}
