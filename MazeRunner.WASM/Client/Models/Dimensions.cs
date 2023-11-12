using System.ComponentModel.DataAnnotations;

namespace MazeRunner.WASM.Client.Models;

public class Dimensions
{
    [Required]
    [Range(5, 150)]
    public int? Width { get; set; }
    [Required]
    [Range(5, 150)]
    public int? Height { get; set; }
}
