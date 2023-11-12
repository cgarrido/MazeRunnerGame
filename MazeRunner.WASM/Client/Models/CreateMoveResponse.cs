namespace MazeRunner.WASM.Client.Models;

public class CreateMoveResponse
{
    public Game? Game { get; set; }
    public MazeBlockView? MazeBlockView { get; set; }
}
