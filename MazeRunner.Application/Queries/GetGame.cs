using FluentValidation;
using MazeRunner.Application.Commands;
using MazeRunner.Domain;
using MediatR;

namespace MazeRunner.Application.Queries;

public class GetGameQuery : IRequest<Tuple<Game, MazeCell>>
{
    public Guid MazeId { get; set; }
    public Guid GameId { get; set; }
}

public class GetGameQueryValidator : AbstractValidator<GetGameQuery>
{
    //TODO existe el maze??? existe el game? tiene sentido? o lanzo error abajo si no hay game
}

public class GetGameQueryHandler : IRequestHandler<GetGameQuery, Tuple<Game, MazeCell>>
{
    public IGamesRepository _gamesRepository { get; set; }
    public IMazesRepository _mazesRepository { get; set; }
    public GetGameQueryHandler(IGamesRepository gamesRepository, IMazesRepository mazesRepository)
    {
        _mazesRepository = mazesRepository ?? throw new ArgumentNullException(nameof(mazesRepository));
        _gamesRepository = gamesRepository ?? throw new ArgumentNullException(nameof(gamesRepository));
    }

    public async Task<Tuple<Game, MazeCell>> Handle(GetGameQuery request, CancellationToken cancellationToken)
    {
        //TODO quitar await
        await Task.Delay(500);

        var maze = _mazesRepository.Get(request.MazeId);
        var game = _gamesRepository.Get(request.GameId);

        if (game == null && maze != null)
        {
            var currentCell = maze.Cells![game!.CurrentPositionX, game.CurrentPositionY];
            return new Tuple<Game, MazeCell>(game, currentCell);
        }
        throw new Exception("Game or maze not founds");
    }
}
