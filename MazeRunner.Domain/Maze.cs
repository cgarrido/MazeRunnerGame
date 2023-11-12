namespace MazeRunner.Domain;

public class Maze
{
	public Maze()
	{
		MazeId = Guid.NewGuid();
	}
    public Guid MazeId { get; set; }
	public MazeDimensions? Dimensions { get; set; }

	public MazeCell[,]? Cells { get; set; }
}