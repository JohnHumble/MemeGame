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
        readonly Texture2D texture;
        readonly int wallSize;
        private Vector2 mid, difference;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="texture"> terrain textur file.</param>
        /// <param name="size">The size of the wall items. size * 2 should not be >= to the hero size / 2 </param>
        public WallCollection(Texture2D texture,int size)
        {
            this.texture = texture;
            wallSize = size;
            mid = difference = new Vector2(0, 0);
        }

        public WallCollection(Texture2D texture, Map map)
        {
            this.texture = texture;
            loadFromMap(map);
            mid = difference = new Vector2(0, 0);
        }

        public void loadFromMap(Map map)
        {
            Clear();
            foreach (var data in map.walls)
            {
                Wall next = new Wall(data, texture);
                Add(next);
            }

            CalculateMidDifference();
        }

        public void createFloor(int x, int y, int count)
        {
            for(int i = 0; i < count; i++)
            {
                Wall next = new Wall(x + (i * wallSize), y, wallSize, wallSize, texture);
                Add(next);
            }

            CalculateMidDifference();
        }

        public void CreateWall(int x, int y, int count)
        {
            for (int i = 0; i < count; i++)
            {
                Wall next = new Wall(x, y + (i * wallSize), wallSize, wallSize, texture);
                Add(next);
            }
            CalculateMidDifference();
        }

        public void CreateBlock(int x, int y, int width, int height)
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
                    if (Intersects(next.Area) == new Rectangle())
                    {
                        Add(next);
                    }
                }
            }
            CalculateMidDifference();
        }

        public Rectangle Intersects(Rectangle other)
        {
            Rectangle rec = new Rectangle();
            int distance = int.MaxValue;
            foreach (var wall in this)
            {
                if (wall.Area.Intersects(other))
                {
                    int test = Math.Abs(wall.Area.Y - other.Y);
                    if (test < distance)
                    {
                        rec = wall.Area;
                        distance = test;
                    }
                }
            }
            return rec;
        }

        public bool TestDamage(Rectangle hit, int damage)
        {
            for (int i = 0; i < Count; i++)
            {
                if (hit.Intersects(this[i].Area))
                {
                    if (this[i].Damage(damage))
                    {
                        RemoveAt(i);
                        i--;
                    }
                    return true;
                }
            }
            return false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var wall in this)
            {
                wall.Draw(spriteBatch);
            }
        }

        private void CalculateMidDifference()
        {
            Vector2 mid = new Vector2(0, 0);
            foreach (var wall in this)
            {
                mid += wall.GetVector();
            }

            mid.X /= Count;
            mid.Y /= Count;

            this.mid = mid;

            float maxY = int.MinValue;
            float maxX = int.MinValue;
            float minY = int.MaxValue;
            float minX = int.MaxValue;
            foreach (var wall in this)
            {
                maxX = Math.Max(maxX, wall.GetVector().X);
                maxY = Math.Max(maxY, wall.GetVector().Y);
                minX = Math.Min(minX, wall.GetVector().X);
                minY = Math.Min(minY, wall.GetVector().Y);
            }
            difference = new Vector2(Math.Abs(maxX - minX), Math.Abs(maxY - minY));
        }
        
        public Vector2 GetDifference()
        {
            return difference;
        }

        public Vector2 GetMid()
        {
            return mid;
        }
    }
}
