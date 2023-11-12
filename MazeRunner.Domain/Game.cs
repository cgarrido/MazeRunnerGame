namespace MazeRunner.Domain;

public class Game
{
    public Guid MazeId { get; set; }
    public Guid GameId { get; set; }
    public bool Completed { get; set; }
    public int CurrentPositionX { get; set; }
    public int CurrentPositionY { get; set; }
}
