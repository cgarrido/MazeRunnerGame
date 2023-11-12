using FluentValidation;
using MazeRunner.Application.Models;
using MazeRunner.Domain;
using MediatR;

namespace MazeRunner.Application.Commands;

public class CreateGameCommand : IRequest<Game>
{
    public Guid MazeId { get; set; }
    public GameOperationType Operation { get; set; }
}
public class CreateGameCommandValidator : AbstractValidator<CreateGameCommand>
{
    //TODO existe el maze???
}
public class CreateGameCommandHandler : IRequestHandler<CreateGameCommand, Game>
{
    public IGamesRepository _gamesRepository { get; set; }
    public IMazesRepository _mazesRepository { get; set; }
    public CreateGameCommandHandler(IGamesRepository gamesRepository, IMazesRepository mazesRepository)
    {
        _mazesRepository = mazesRepository ?? throw new ArgumentNullException(nameof(mazesRepository));
        _gamesRepository = gamesRepository ?? throw new ArgumentNullException(nameof(gamesRepository));
    }

    public async Task<Game> Handle(CreateGameCommand cmd, CancellationToken cancellationToken)
    {
        //TODO quitar await
        await Task.Delay(500);

        var maze = _mazesRepository.Get(cmd.MazeId);
        if(maze == null) throw new Exceptions.NotFoundException("Maze", cmd.MazeId);

        var game = new Game()
        {
            CurrentPositionX = 0,
            CurrentPositionY = 0,
            MazeId = cmd.MazeId,
            GameId = Guid.NewGuid(),
            Completed = false
        };

        _gamesRepository.Add(game);
        return game;
    }
}
