using MazeRunner.Application.Models;
using MazeRunner.Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MazeRunner.Application.Commands;

public class CreateGameCommand : IRequest<Game>
{
    public Guid MazeId { get; set; }
    public GameOperationType Operation { get; set; }
}

public class CreateGameCommandHandler : IRequestHandler<CreateGameCommand, Game>
{
    public IGamesRepository _gamesRepository { get; set; }
    public IMazesRepository _mazesRepository { get; set; }
    private ILogger<CreateGameCommandHandler> _logger { get; set; }
    public CreateGameCommandHandler(IGamesRepository gamesRepository, IMazesRepository mazesRepository, ILogger<CreateGameCommandHandler> logger)
    {
        _mazesRepository = mazesRepository ?? throw new ArgumentNullException(nameof(mazesRepository));
        _gamesRepository = gamesRepository ?? throw new ArgumentNullException(nameof(gamesRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<Game> Handle(CreateGameCommand cmd, CancellationToken cancellationToken)
    {
        return await Task.Run(() =>
        {
            _logger.LogDebug("Creating game from maze with MazeId='{0}'", cmd.MazeId);
            var maze = _mazesRepository.Get(cmd.MazeId);
            if (maze == null) throw new Exceptions.NotFoundException("Maze", cmd.MazeId);

            var game = new Game()
            {
                CurrentPositionX = 0,
                CurrentPositionY = 0,
                MazeId = cmd.MazeId,
                GameId = Guid.NewGuid(),
                Completed = false
            };

            _logger.LogInformation("Game '{0}' created from maze with MazeId='{1}'", game.GameId, cmd.MazeId);
            _gamesRepository.Add(game);
            return game;
        });
    }
}
