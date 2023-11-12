namespace MazeRunner.API.Models;

public class MazeBlockView
{
    public int CoordX { get; set; }
    public int CoordY { get; set; }
    public bool EastBlocked { get; set; }
    public bool WestBlocked { get; set; }
    public bool NorthBlocked { get; set; }
    public bool SouthBlocked { get; set; }
}
