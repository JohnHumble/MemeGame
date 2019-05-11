using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemeGame
{
    class Wall
    {
        public Rectangle Area { get; set; }
        public int Health { get; set; }

        private Texture2D texture;

        /// <summary>
        /// Constructor to create a wall objectect
        /// </summary>
        /// <param name="x">X location</param>
        /// <param name="y">Y location</param>
        /// <param name="width">Width of the wall block</param>
        /// <param name="height">Height of the wall block</param>
        /// <param name="texture">Texture for the wall block</param>
        /// <param name="health">number of hitpoints for the block </param>
        public Wall(int x, int y, int width, int height, Texture2D texture, int health = 10)
        {
            this.texture = texture;
            Area = new Rectangle(x, y, width, height);
            Health = health;
        }

        public Wall(WallData wall, Texture2D texture, int health = 10)
        {
            this.texture = texture;
            Health = health;

            Area = new Rectangle(wall.x, wall.y, wall.width, wall.height);
        }

        /// <summary>
        /// Method that draws the wall object. 
        /// </summary>
        /// <param name="spriteBatch">an already begun SpriteBatch to use to draw the object</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Area, Color.White);
        }

        public Point GetPoint()
        {
            return new Point(Area.X, Area.Y);
        }

        public int distance(Point point)
        {
            return distance(point.X,point.Y);
        }
        public int distance(int x, int y)
        {
            return Math.Abs(x - Area.X) + Math.Abs(y - Area.Y);
        }

        public WallData GetWallData()
        {
            return new WallData(Area.X, Area.Y, Area.Width, Area.Height);
        }
    }
}
