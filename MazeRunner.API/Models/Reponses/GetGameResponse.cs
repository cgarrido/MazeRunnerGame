namespace MazeRunner.API.Models.Reponses;

public class GetGameResponse
{
    public Game? Game { get; set; }
    public MazeBlockView? MazeBlockView { get; set; }
}
