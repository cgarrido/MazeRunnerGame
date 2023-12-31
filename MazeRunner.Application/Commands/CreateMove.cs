﻿using MazeRunner.Application.Exceptions;
using MazeRunner.Application.Models;
using MazeRunner.Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MazeRunner.Application.Commands;

public class CreateMoveCommand : IRequest<Tuple<Game, MazeCell>>
{
    public Guid MazeId { get; set; }
    public Guid GameId { get; set; }
    public GameOperationType Operation { get; set; }
}

public class CreateMoveCommandHandler : IRequestHandler<CreateMoveCommand, Tuple<Game, MazeCell>>
{
    public IGamesRepository _gamesRepository { get; set; }
    public IMazesRepository _mazesRepository { get; set; }
    private ILogger<CreateMoveCommandHandler> _logger { get; set; }

    public CreateMoveCommandHandler(IGamesRepository gamesRepository, IMazesRepository mazesRepository, ILogger<CreateMoveCommandHandler> logger)
    {
        _mazesRepository = mazesRepository ?? throw new ArgumentNullException(nameof(mazesRepository));
        _gamesRepository = gamesRepository ?? throw new ArgumentNullException(nameof(gamesRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<Tuple<Game, MazeCell>> Handle(CreateMoveCommand cmd, CancellationToken cancellationToken)
    {
        return await Task.Run(() =>
        {
            _logger.LogDebug("Creating move with MazeId='{0}' GameId='{1}' Operation='{2}'", cmd.MazeId, cmd.GameId, cmd.Operation);
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
                    else if(cmd.Operation.Equals(GameOperationType.Start)) currentCell = maze.Cells[0, 0];
                    else throw new MoveException();

                    game.CurrentPositionX = currentCell.CoordX;
                    game.CurrentPositionY = currentCell.CoordY;
                    game.Completed = game.CurrentPositionX.Equals(maze.Dimensions!.Height - 1) && game.CurrentPositionY.Equals(maze.Dimensions!.Width - 1);
                    _gamesRepository.Add(game);

                    return new Tuple<Game, MazeCell>(game, currentCell);
                }
                else throw new NotFoundException("Maze", cmd.MazeId);
            }
            else throw new NotFoundException("Game", cmd.GameId);

        });
    }
}