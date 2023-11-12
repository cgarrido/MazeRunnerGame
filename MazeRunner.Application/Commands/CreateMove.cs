using FluentValidation;
using MazeRunner.Application.Models;
using MazeRunner.Domain;
using MediatR;

namespace MazeRunner.Application.Commands;

public class CreateMoveCommand : IRequest<Tuple<Game, MazeCell>>
{
    public Guid MazeId { get; set; }
    public Guid GameId { get; set; }
    public GameOperationType Operation { get; set; }
}

public class CreateMoveCommandValidator : AbstractValidator<CreateMoveCommand>
{
    //Movimiento válido?? mejor en el otro lado no? si
}
public class CreateMoveCommandHandler : IRequestHandler<CreateMoveCommand, Tuple<Game, MazeCell>>
{
    public IGamesRepository _gamesRepository { get; set; }
    public IMazesRepository _mazesRepository { get; set; }
    public CreateMoveCommandHandler(IGamesRepository gamesRepository, IMazesRepository mazesRepository)
    {
        _mazesRepository = mazesRepository ?? throw new ArgumentNullException(nameof(mazesRepository));
        _gamesRepository = gamesRepository ?? throw new ArgumentNullException(nameof(gamesRepository));
    }

    public async Task<Tuple<Game, MazeCell>> Handle(CreateMoveCommand cmd, CancellationToken cancellationToken)
    {
        //TODO quitar await
        await Task.Delay(500);

        var game = _gamesRepository.Get(cmd.GameId);

        if (game != null)
        {
            var maze = _mazesRepository.Get(game!.MazeId);

            if (maze != null)
            {
                var currentCell = maze.Cells![game.CurrentPositionX, game.CurrentPositionY];

                if (cmd.Operation.Equals(GameOperationType.GoNorth) && !currentCell.NorthBlocked) currentCell = maze.Cells[game.CurrentPositionX, game.CurrentPositionY - 1];
                else if (cmd.Operation.Equals(GameOperationType.GoSouth) && !currentCell.SouthBlocked) currentCell = maze.Cells[game.CurrentPositionX, game.CurrentPositionY + 1];
                else if (cmd.Operation.Equals(GameOperationType.GoWest) && !currentCell.WestBlocked) currentCell = maze.Cells[game.CurrentPositionX - 1, game.CurrentPositionY];
                else if (cmd.Operation.Equals(GameOperationType.GoEast) && !currentCell.EastBlocked) currentCell = maze.Cells[game.CurrentPositionX + 1, game.CurrentPositionY];
                else throw new Exception("Wrong move!");

                game.CurrentPositionX = currentCell.CoordX;
                game.CurrentPositionY = currentCell.CoordY;
                game.Completed = game.CurrentPositionX.Equals(maze.Dimensions!.Height - 1) && game.CurrentPositionY.Equals(maze.Dimensions!.Width - 1);
                _gamesRepository.Add(game);

                return new Tuple<Game, MazeCell>(game, currentCell);
            }
        }

        throw new Exception("Game or maze not founds");
    }
}