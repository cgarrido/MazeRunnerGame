using System.Text.Json.Serialization;

namespace MazeRunner.API.Shared;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum GameOperationType
{
    Start,
    GoNorth,
    GoSouth,
    GoEast,
    GoWest
}
