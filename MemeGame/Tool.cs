using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemeGame
{
    class Tool
    {
        public static float distance(Vector2 j, Vector2 i)
        {
            float x = Math.Abs(i.X - j.X);
            float y = Math.Abs(i.Y - j.Y);
            return x + y;
        }

        public static int distance(Point j, Point i)
        {
            int x = Math.Abs(i.X - j.X);
            int y = Math.Abs(i.Y - j.Y);
            return x + y;
        }
    }
}
