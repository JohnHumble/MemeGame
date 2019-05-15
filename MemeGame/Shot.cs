using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemeGame
{
    class Shot
    {
        private Texture2D texture;
        Rectangle rectangle;

        private int speed;
        private int damage;
        private int decay;

        public Shot(Texture2D texture, Rectangle rectangle, int damage, int speed = 10, int decay = 240)
        {
            this.texture = texture;
            this.rectangle = rectangle;
            this.speed = speed;
            this.damage = damage;
            this.decay = decay;
        }

        /// <summary>
        /// Update shot "bullets"
        /// </summary>
        /// <param name="players">players list used for damage</param>
        /// <param name="walls">walls list used for damage</param>
        /// <returns>returns true if the shot should be removed</returns>
        public bool Update(PlayerCollection players, WallCollection walls)
        {
            rectangle.X += speed;
            Rectangle hit = new Rectangle(rectangle.Center.X, rectangle.Y, speed, rectangle.Width);
            
            // test players
            

            
            // test walls
            if (walls.TestDamage(hit, damage))
            {
                return true;
            }

            if (decay < 0)
            {
                return true;
            }

            return false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rectangle, Color.Yellow);
        }
    }
}
