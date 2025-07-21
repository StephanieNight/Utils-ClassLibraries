namespace TerminalUX
{
  public class ConsolePoint
  {
    public ConsolePoint() { x = 0; y = 0; }
    public ConsolePoint(int x, int y)
    {
      this.x = x;
      this.y = y;
    }
    public int x { get; set; }
    public int y { get; set; }
  }
}
