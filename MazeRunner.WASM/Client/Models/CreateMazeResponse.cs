namespace MazeRunner.WASM.Client.Models;

public class CreateMazeResponse
{
    public Guid MazeUid { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
}

