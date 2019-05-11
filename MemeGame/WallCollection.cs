using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemeGame
{
    class WallCollection : List<Wall>
    {
        Texture2D texture;
        int wallSize;

        public WallCollection(Texture2D texture,int size)
        {
            this.texture = texture;
            wallSize = size;
        }

        public void createFloor(int x, int y, int count)
        {
            for(int i = 0; i < count; i++)
            {
                Wall next = new Wall(x + (i * wallSize), y, wallSize, wallSize, texture);
                Add(next);
            }
        }

        public void createWall(int x, int y, int count)
        {
            for (int i = 0; i < count; i++)
            {
                Wall next = new Wall(x, y + (i * wallSize), wallSize, wallSize, texture);
                Add(next);
            }
        }

        public void createBlock(int x, int y, int width, int height)
        {
            int cols = width / wallSize;
            int rows = height / wallSize;

            x -= x % wallSize;
            y -= y % wallSize;

            for (int i = 0; i < cols; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    Wall next = new Wall(x + i * wallSize, y + j * wallSize, wallSize, wallSize,texture);
                    Add(next);
                }
            }
        }

        public Rectangle Intersects(Rectangle other)
        {
            foreach (var wall in this)
            {
                if (wall.Area.Intersects(other))
                {
                    return wall.Area;
                }
            }
            return new Rectangle();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var wall in this)
            {
                wall.Draw(spriteBatch);
            }
        }
    }
}
