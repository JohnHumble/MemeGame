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

        private Point speed;
        private int damage;
        private int decay;

        Hero owner;

        public Shot(Texture2D texture, Rectangle rectangle, Hero owner, int damage, int speed = 42, int decay = 240)
        {
            this.texture = texture;
            this.rectangle = rectangle;
            this.speed = new Point(speed,0);
            this.damage = damage;
            this.decay = decay;
            this.owner = owner;
        }
        public Shot(Texture2D texture, Point location, Point speed, Hero owner, int height, int damage, int decay = 240)
        {
            this.texture = texture;
            this.rectangle = new Rectangle(location,new Point(Math.Abs(speed.X),height));
            this.speed = speed;
            this.damage = damage;
            this.decay = decay;
            this.owner = owner;
        }

        /// <summary>
        /// Update shot "bullets"
        /// </summary>
        /// <param name="players">players list used for damage</param>
        /// <param name="walls">walls list used for damage</param>
        /// <returns>returns true if the shot should be removed</returns>
        public bool Update(PlayerCollection players, WallCollection walls)
        {
            if (decay < 0)
            {
                return true;
            }

            Rectangle hit = new Rectangle(rectangle.Center.X, rectangle.Y, Math.Abs(speed.X), Math.Abs(speed.Y));

            // test players
            if (players.TestHit(hit, damage,owner))
            {
                return true;
            }

            
            // test walls
            if (walls.TestDamage(hit, damage))
            {
                return true;
            }


            rectangle.X += speed.X;
            rectangle.Y += speed.Y;

            return false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rectangle, Color.Yellow);
        }
    }
}
