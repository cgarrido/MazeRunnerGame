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

    public MazeController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    [HttpPost()]
    public async Task<ActionResult> Post([FromBody] CreateMazeRequest dimensions, CancellationToken cancellationToken)
    {
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
