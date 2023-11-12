namespace MazeRunner.Domain;

public interface IGamesRepository
{
    IEnumerable<Game> Get();
    Game? Get(Guid id);
    Game Add(Game obj);
    void Delete(Guid id);
}
