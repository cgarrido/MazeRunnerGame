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
    public IGamesRepository _repository { get; set; }
    public CreateGameCommandHandler(IGamesRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<Game> Handle(CreateGameCommand cmd, CancellationToken cancellationToken)
    {
        //TODO quitar await
        await Task.Delay(500);

        var game = new Game()
        {
            CurrentPositionX = 0,
            CurrentPositionY = 0,
            MazeId = cmd.MazeId,
            GameId = Guid.NewGuid(),
            Completed = false
        };

        _repository.Add(game);
        return game;
    }
}
