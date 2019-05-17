using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemeGame
{
    [Serializable]

    class PointData
    {
        public int x, y;

        public PointData(int x, int y)
        {
            this.x = x; this.y = y;
        }
    }
}
