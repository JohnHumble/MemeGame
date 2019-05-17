using Microsoft.Xna.Framework;
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
        public List<PointData> startLocations;
        public List<PointData> gunLocations;

        public Map(WallCollection collection, List<Point> startLocations, List<Point> gunLocations)
        {
            // save wall data
            walls = new List<WallData>();
            foreach (var wall in collection)
            {
                walls.Add(wall.GetWallData());
            }

            // save start locations


            // save gun locations

        
        }
    }
}
