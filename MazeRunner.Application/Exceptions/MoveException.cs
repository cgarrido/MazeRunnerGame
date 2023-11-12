using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeRunner.Application.Exceptions;

public class MoveException : Exception
{
    public MoveException()
        : base("Move not allowed!")
    {
    }

    public MoveException(string message)
        : base(message)
    {
    }

    public MoveException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

}
