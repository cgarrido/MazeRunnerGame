namespace MazeRunner.API.Shared.Reponses;

public class CreateGameResponse
{
    public Guid MazeId { get; set; }
    public Guid GameId { get; set; }
    public bool Completed { get; set; }
    public int CurrentPositionX { get; set; }
    public int CurrentPositionY { get; set; }
}
