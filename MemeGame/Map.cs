using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemeGame
{
    [Serializable]

    class Map
    {
        public List<Wall> walls;

        public Map(WallCollection walls)
        {
            this.walls = new List<Wall>();
            foreach (var wall in walls)
            {
                this.walls.Add(wall);
            }
        }
    }
}
