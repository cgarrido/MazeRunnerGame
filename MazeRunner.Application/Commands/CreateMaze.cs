using FluentValidation;
using MazeRunner.Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MazeRunner.Application.Commands
{
    public class CreateMazeCommand : IRequest<Maze>
    {
        public MazeDimensions? Dimensions { get; set; }
    }

    public class CreateMazeCommandValidator : AbstractValidator<CreateMazeCommand>
    {
        private const int MIN_WIDTH = 5;
        private const int MIN_HEIGHT = 5;
        private const int MAX_WIDTH = 150;
        private const int MAX_HEIGHT = 150;
        public CreateMazeCommandValidator()
        {
            RuleFor(x => x.Dimensions).NotNull().NotEmpty();
            RuleFor(x => x.Dimensions!.Width).GreaterThanOrEqualTo(MIN_WIDTH).LessThanOrEqualTo(MAX_WIDTH);
            RuleFor(x => x.Dimensions!.Height).GreaterThanOrEqualTo(MIN_HEIGHT).LessThanOrEqualTo(MAX_HEIGHT);
        }
    }

    public class CreateMazeCommandHandler : IRequestHandler<CreateMazeCommand, Maze>
    {
        public IMazesRepository _repository { get; set; }
        private ILogger<CreateMazeCommandHandler> _logger { get; set; }
        public CreateMazeCommandHandler(IMazesRepository repository, ILogger<CreateMazeCommandHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Maze> Handle(CreateMazeCommand cmd, CancellationToken cancellationToken)
        {
            return await Task.Run(() =>
            {
                _logger.LogDebug("Creating maze with width={0} height={1}", cmd.Dimensions!.Width, cmd.Dimensions!.Height);
                var maze = new Maze()
                {
                    Dimensions = cmd.Dimensions
                };
                CreateCells(maze);

                _repository.Add(maze);

                _logger.LogInformation("Game '{0}' created", maze.MazeId);
                return maze;
            });
        }

        private void CreateCells(Maze maze)
        {
            //Generate empty maze with borders
            _logger.LogDebug("Creating empty cells with border");
            maze.Cells = new MazeCell[maze.Dimensions!.Width, maze.Dimensions!.Height];
            for (int positionY = 0; positionY < maze.Dimensions!.Height; positionY++)
            {
                for (int positionX = 0; positionX < maze.Dimensions!.Width; positionX++)
                {
                    var cell = new MazeCell()
                    {
                        CoordX = positionX,
                        CoordY = positionY,
                        WestBlocked = (positionX == 0),
                        EastBlocked = (positionX == maze.Dimensions.Width - 1),
                        NorthBlocked = (positionY == 0),
                        SouthBlocked = (positionY == maze.Dimensions.Height - 1)
                    };
                    maze.Cells[positionX, positionY] = (cell);
                }
            }

            //Calculating tree with one path (0, 0) => (h, w)
            _logger.LogDebug("Calculating success path and creating internal walls");
            var currentNode = maze.Cells[0, 0];
            maze.Cells[currentNode.CoordX, currentNode.CoordY].Visited = true;
            var nextNodes = GetNextNodes(maze.Cells, currentNode);

            //N nodes => N loops
            while (nextNodes.Any())
            {
                var rnd = new Random();
                var rndValue = rnd.Next(0, nextNodes.Count - 1);

                //Choosing one node to visit (random) and creating walls
                currentNode = nextNodes.ElementAt(rndValue);
                nextNodes.Remove(currentNode);
                maze.Cells[currentNode.CoordX, currentNode.CoordY].Visited = true;
                CreateWalls(maze.Cells, currentNode);

                //Next nodes to visit
                nextNodes = nextNodes.Union(GetNextNodes(maze.Cells, currentNode)).ToList();
            }
        }

        private IList<MazeCell> GetNextNodes(MazeCell[,] nodes, MazeCell node)
        {
            var result = new List<MazeCell>();
            if (!node.NorthBlocked) AddNextNode(result, node, nodes[node.CoordX, node.CoordY - 1], node.NorthBlocked);
            if (!node.SouthBlocked) AddNextNode(result, node, nodes[node.CoordX, node.CoordY + 1], node.SouthBlocked);
            if (!node.WestBlocked) AddNextNode(result, node, nodes[node.CoordX - 1, node.CoordY], node.WestBlocked);
            if (!node.EastBlocked) AddNextNode(result, node, nodes[node.CoordX + 1, node.CoordY], node.EastBlocked);

            return result;
        }

        private void AddNextNode(IList<MazeCell> nextNodes, MazeCell node, MazeCell nextNode, bool blocked)
        {
            if (!blocked && !nextNode.Visited)
            {
                nextNode.Parent = node;
                nextNodes.Add(nextNode);
            }
        }

        private void CreateWalls(MazeCell[,] nodes, MazeCell node)
        {
            if (!node.NorthBlocked)
            {
                var nodeTo = nodes[node.CoordX, node.CoordY - 1];
                if (nodeTo != node.Parent && nodeTo.Visited)
                {
                    nodes[nodeTo.CoordX, nodeTo.CoordY].SouthBlocked = true;
                    nodes[node.CoordX, node.CoordY].NorthBlocked = true;
                }
            }
            if (!node.SouthBlocked)
            {
                var nodeTo = nodes[node.CoordX, node.CoordY + 1];
                if (nodeTo != node.Parent && nodeTo.Visited)
                {
                    nodes[nodeTo.CoordX, nodeTo.CoordY].NorthBlocked = true;
                    nodes[node.CoordX, node.CoordY].SouthBlocked = true;
                }
            }
            if (!node.WestBlocked)
            {
                var nodeTo = nodes[node.CoordX - 1, node.CoordY];
                if (nodeTo != node.Parent && nodeTo.Visited)
                {
                    nodes[nodeTo.CoordX, nodeTo.CoordY].EastBlocked = true;
                    nodes[node.CoordX, node.CoordY].WestBlocked = true;
                }
            }
            if (!node.EastBlocked)
            {
                var nodeTo = nodes[node.CoordX + 1, node.CoordY];
                if (nodeTo != node.Parent && nodeTo.Visited)
                {
                    nodes[nodeTo.CoordX, nodeTo.CoordY].WestBlocked = true;
                    nodes[node.CoordX, node.CoordY].EastBlocked = true;
                }
            }
        }
    }
}
