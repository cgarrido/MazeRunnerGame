namespace MazeRunner.Domain;

public interface IMazesRepository
{
    IEnumerable<Maze> Get();
    Maze? Get(Guid id);
    Maze Add(Maze obj);
    void Delete(Guid id);
}