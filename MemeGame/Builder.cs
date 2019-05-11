using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemeGame
{
    class Builder
    {
        private WallCollection walls;

        private int radius;

        public Builder(WallCollection walls)
        {
            this.walls = walls;
            radius = 32;
        }

        public void building(MouseState mouse)
        {
            // create tiles with left click
            if (mouse.LeftButton == ButtonState.Pressed)
            {
                walls.createBlock(mouse.X - radius / 2, mouse.Y - radius / 2, radius, radius);
            }

            // remove tiles with right click
            if (mouse.RightButton == ButtonState.Pressed)
            {
                for (int i = 0; i < walls.Count; i++)
                {
                    Wall wall = walls[i];
                    if (wall.distance(mouse.X,mouse.Y) < radius){
                        walls.RemoveAt(i);
                        i--;
                    }
                }
            }
        }
    }
}
