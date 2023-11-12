using System.Text.Json.Serialization;

namespace MazeRunner.API.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum GameOperationType
{
    Start,
    GoNorth,
    GoSouth,
    GoEast,
    GoWest
}
