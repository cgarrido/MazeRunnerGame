using MazeRunner.API.Shared.Reponses;
using MazeRunner.API.Shared.Requests;
using MazeRunner.Application.Commands;
using MazeRunner.Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MazeRunner.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MazeController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<MazeController> _logger;
    public MazeController(IMediator mediator, ILogger<MazeController> logger)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpPost()]
    public async Task<ActionResult> Post([FromBody] CreateMazeRequest dimensions, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Create Maze request received");
        var maze = await _mediator.Send(new CreateMazeCommand()
        {
            Dimensions = new MazeDimensions()
            {
                Width = dimensions!.Width,
                Height = dimensions!.Height
            }
        }, cancellationToken);

        var result = new CreateMazeResponse()
        {
            MazeUid = maze.MazeId,
            Width = maze.Dimensions!.Width,
            Height = maze.Dimensions!.Height
        };
        return Ok(result);
    }
}
