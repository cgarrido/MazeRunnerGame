namespace MazeRunner.API.Shared.Reponses;

public class CreateGameResponse
{
    public Guid MazeUid { get; set; }
    public Guid GameUid { get; set; }
    public bool Completed { get; set; }
    public int CurrentPositionX { get; set; }
    public int CurrentPositionY { get; set; }
}
