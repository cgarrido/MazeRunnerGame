﻿using MazeRunner.API.Shared;
using MazeRunner.API.Shared.Reponses;
using MazeRunner.API.Shared.Requests;
using MazeRunner.Application.Commands;
using MazeRunner.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MazeRunner.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GameController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<GameController> _logger;

    public GameController(IMediator mediator, ILogger<GameController> logger)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpPost("{mazeUid}")]
    public async Task<ActionResult> Post([FromBody] CreateGameRequest game, Guid mazeUid, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Create Game request received");
        if (game.Operation.Equals(GameOperationType.Start))
        {
            var gameData = await _mediator.Send(new CreateGameCommand()
            {
                MazeId = mazeUid
            });
            var result = new CreateGameResponse()
            {
                MazeUid = gameData.MazeId,
                GameUid = gameData.GameId,
                Completed = gameData.Completed,
                CurrentPositionX = gameData.CurrentPositionX,
                CurrentPositionY = gameData.CurrentPositionY,
            };
            return Ok(result);
        }
        return BadRequest();
    }

    [HttpGet("{mazeUid}/{gameUid}")]
    public async Task<ActionResult> Get(Guid mazeUid, Guid gameUid, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Get Game request received");
        var gameData = await _mediator.Send(new GetGameQuery()
        {
            MazeId = mazeUid,
            GameId = gameUid
        });

        var result = new GetGameResponse()
        {
            Game = new Game()
            {
                MazeUid = gameData.Item1.MazeId,
                GameUid = gameData.Item1.GameId,
                Completed = gameData.Item1.Completed,
                CurrentPositionX = gameData.Item1.CurrentPositionX,
                CurrentPositionY = gameData.Item1.CurrentPositionY,
            },
            MazeBlockView = new MazeBlockView()
            {
                CoordX = gameData.Item2.CoordX,
                CoordY = gameData.Item2.CoordY,
                NorthBlocked = gameData.Item2.NorthBlocked,
                SouthBlocked = gameData.Item2.SouthBlocked,
                WestBlocked = gameData.Item2.WestBlocked,
                EastBlocked = gameData.Item2.EastBlocked
            }
        };

        return Ok(result);
    }

    [HttpPost("{mazeUid}/{gameUid}")]
    public async Task<ActionResult> PostMove([FromBody] CreateMoveRequest move, Guid mazeUid, Guid gameUid, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Create move request received");
        var gameData = await _mediator.Send(new CreateMoveCommand()
        {
            MazeId = mazeUid,
            GameId = gameUid,
            Operation = (Application.Models.GameOperationType)move.Operation!
        });

        var result = new GetGameResponse()
        {
            Game = new Game()
            {
                MazeUid = gameData.Item1.MazeId,
                GameUid = gameData.Item1.GameId,
                Completed = gameData.Item1.Completed,
                CurrentPositionX = gameData.Item1.CurrentPositionX,
                CurrentPositionY = gameData.Item1.CurrentPositionY,
            },
            MazeBlockView = new MazeBlockView()
            {
                CoordX = gameData.Item2.CoordX,
                CoordY = gameData.Item2.CoordY,
                NorthBlocked = gameData.Item2.NorthBlocked,
                SouthBlocked = gameData.Item2.SouthBlocked,
                WestBlocked = gameData.Item2.WestBlocked,
                EastBlocked = gameData.Item2.EastBlocked
            }
        };

        return Ok(result);
    }
}
