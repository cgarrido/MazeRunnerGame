using MazeRunner.Domain;

namespace MazeRunner.Data;

public class MazesRepository : IMazesRepository
{
    private static IList<Maze> _mazes = new List<Maze>();
    public Maze Add(Maze obj)
    {
        if (!_mazes.Any(m => m.MazeId == obj.MazeId))
        {
            _mazes.Add(obj);
        }
        else
        {
            var found = _mazes.Where(v => v.MazeId == obj.MazeId).FirstOrDefault();
            if (found != null) _mazes[_mazes.IndexOf(found)] = obj;
        }

        return obj;
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Maze> Get()
    {
        return _mazes;
    }

    public Maze? Get(Guid id)
    {
        return _mazes.Where(v => v.MazeId == id).FirstOrDefault();
    }
}
