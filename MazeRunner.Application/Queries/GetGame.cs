using FluentValidation;
using MazeRunner.Application.Commands;
using MazeRunner.Application.Exceptions;
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

    public async Task<Tuple<Game, MazeCell>> Handle(GetGameQuery query, CancellationToken cancellationToken)
    {
        return await Task.Run(() =>
        {
            var maze = _mazesRepository.Get(query.MazeId);
            var game = _gamesRepository.Get(query.GameId);

            if (maze == null) throw new NotFoundException("Maze", query.MazeId);
            if (game == null) throw new NotFoundException("Game", query.GameId);

            var currentCell = maze.Cells![game.CurrentPositionX, game.CurrentPositionY];
            return new Tuple<Game, MazeCell>(game, currentCell);

        });
    }
}
