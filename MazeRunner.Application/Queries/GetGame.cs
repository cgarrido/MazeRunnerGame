using MazeRunner.Application.Exceptions;
using MazeRunner.Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MazeRunner.Application.Queries;

public class GetGameQuery : IRequest<Tuple<Game, MazeCell>>
{
    public Guid MazeId { get; set; }
    public Guid GameId { get; set; }
}

public class GetGameQueryHandler : IRequestHandler<GetGameQuery, Tuple<Game, MazeCell>>
{
    private IGamesRepository _gamesRepository { get; set; }
    private IMazesRepository _mazesRepository { get; set; }
    private ILogger<GetGameQueryHandler> _logger { get; set; }
    public GetGameQueryHandler(IGamesRepository gamesRepository, IMazesRepository mazesRepository, ILogger<GetGameQueryHandler> logger)
    {
        _mazesRepository = mazesRepository ?? throw new ArgumentNullException(nameof(mazesRepository));
        _gamesRepository = gamesRepository ?? throw new ArgumentNullException(nameof(gamesRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<Tuple<Game, MazeCell>> Handle(GetGameQuery query, CancellationToken cancellationToken)
    {
        return await Task.Run(() =>
        {
            _logger.LogDebug("Getting game with MazeId='{0}', GameId='{1}'", query.MazeId, query.GameId);
            var maze = _mazesRepository.Get(query.MazeId);
            var game = _gamesRepository.Get(query.GameId);

            if (maze == null) throw new NotFoundException("Maze", query.MazeId);
            if (game == null) throw new NotFoundException("Game", query.GameId);

            var currentCell = maze.Cells![game.CurrentPositionX, game.CurrentPositionY];
            return new Tuple<Game, MazeCell>(game, currentCell);

        });
    }
}
