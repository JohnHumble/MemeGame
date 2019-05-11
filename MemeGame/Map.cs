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
        public List<WallData> walls;

        public Map(WallCollection collection)
        {
            walls = new List<WallData>();
            foreach (var wall in collection)
            {
                this.walls.Add(wall.GetWallData());
            }
        }
    }
}
