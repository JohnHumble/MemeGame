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

        /// <summary>
        /// returns a 
        /// </summary>
        /// <param name="other"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public int TestGround(Rectangle other, int height)
        {
            foreach (var wall in this)
            {
                if (wall.Area.Intersects(other))
                {
                    return wall.Area.Y - height;
                }
            }
            return -1;
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
