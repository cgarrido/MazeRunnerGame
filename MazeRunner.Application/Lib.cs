using System.Reflection;

namespace MazeRunner.Application
{
    public class Lib
    {
        public static Assembly GetAssembly()
        {
            return Assembly.GetExecutingAssembly();
        }
    }
}
