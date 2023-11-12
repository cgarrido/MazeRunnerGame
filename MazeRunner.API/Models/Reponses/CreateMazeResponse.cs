namespace MazeRunner.API.Models.Reponses;

public class CreateMazeResponse
{
    public Guid MazeUid { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
}
