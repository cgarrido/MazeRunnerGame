using MazeRunner.Domain;

namespace MazeRunner.Data;

public class GamesRepositoryFake : IGamesRepository
{
    private static IList<Game> _games = new List<Game>();
    public Game Add(Game obj)
    {
        if (!_games.Any(m => m.GameId == obj.GameId))
        {
            _games.Add(obj);
        }
        else
        {
            var found = _games.Where(v => v.GameId == obj.GameId).FirstOrDefault();
            if (found != null) _games[_games.IndexOf(found)] = obj;
        }

        return obj;
    }

    public void Delete(Guid id)
    {
        var obj = _games.FirstOrDefault(v => v.MazeId == id);
        if (obj != null)
        {
            _games.Remove(obj);
        }
    }

    public IEnumerable<Game> Get()
    {
        return _games;
    }

    public Game? Get(Guid id)
    {
        return _games.Where(v => v.GameId == id).FirstOrDefault();
    }
}
