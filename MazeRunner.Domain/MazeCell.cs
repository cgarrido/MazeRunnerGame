using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeRunner.Domain
{
    public class MazeCell
    {
        public int CoordX { get; set; }
        public int CoordY { get; set; }
        public bool EastBlocked { get; set; }
        public bool WestBlocked { get; set; }
        public bool NorthBlocked { get; set; }
        public bool SouthBlocked { get; set; }
        public bool Visited { get; set; }
        public MazeCell? Parent { get; set; }
    }
}
