namespace TerminalUX.Table
{
  public class TableSize
  {
    public TableSize()
    {
      Location = new ConsolePoint(0, 0);
      Widht = 0;
      Height = 0;
    }
    public ConsolePoint Location { get; set; }
    public int Widht { get; set; }
    public int Height { get; set; }
  }
}
