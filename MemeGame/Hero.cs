using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemeGame
{
    class Hero
    {
        // Constant values
        const int IMAGE_SIZE = 118; //size of each image in the image sheet.
        const int MAX_SPEED = 32; // the maximum velocity on the ground.

        // location and physics based
        public Rectangle Rec { get; private set; }
        public Point Velocity { get; private set; }
        public int AccelX { get; set; }
        public int AccelY { get; set; }
        public bool OnGround { get; private set; }

        Texture2D texture;
        Rectangle source;

        // number of hitpoints
        public int Health { get; private set; }
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="location">start location of the hero</param>
        /// <param name="width">width of the hero</param>
        /// <param name="height">height of the hero</param>
        /// <param name="health">the amout of hp for the hero</param>
        /// <param name="texture">the texture of the hero</param>
        /// <param name="acceleration">Starting acceleration(gravity)</param>
        public Hero(Point location, int width, int height, int health, Texture2D texture, int acceleration_x, int acceleration_y)
        {
            Rec = new Rectangle(location, new Point(width, height));
            source = new Rectangle(0, 0, IMAGE_SIZE, IMAGE_SIZE);
            this.texture = texture;
            Health = health;
            AccelX = acceleration_x;
            AccelY = acceleration_y;
            OnGround = false;
        }

        public void Update(int gravity, WallCollection walls)
        {
            // Save Velocity and Rec values to be manipulated
            Rectangle rectangle = Rec;
            Point velocity = Velocity;

            // test to see if on the ground
            int offset = walls.TestGround(rectangle, rectangle.Y);
            if (offset >= 0)
            {
                OnGround = true;
                rectangle.Y += offset - rectangle.Height; // do something about the magic number
            }

            // make no downard Y direction if on ground
            if (OnGround && AccelY >= 0)
            {
                AccelY = 0;
                velocity.Y = 0;
            }
            else
            {
                AccelY = gravity;
            }

            // update velocity based on acceeleration
            velocity.X += AccelX;
            velocity.Y += AccelY;
            // uppdate the rectangle based on velocity
            rectangle.X += velocity.X;
            rectangle.Y += velocity.Y;

            // reset Velocity and Rec values
            Velocity = velocity;
            Rec = rectangle;
        }

        /// <summary>
        /// Method used to have the hero jump. It will only jump if it is on the ground.
        /// </summary>
        /// <param name="force">value of velocity to initially start moving up.</param>
        public void jump(int force)
        {
            if(OnGround)
                Velocity = new Point(Velocity.X, -force);
        }

        /// <summary>
        /// Method used to move the hero to the right.
        /// </summary>
        /// <param name="force">used for acceleration in that direction</param>
        public void moveRight(int force)
        {
            AccelX = force;
        }

        /// <summary>
        /// Method used to move the hero to the left
        /// </summary>
        /// <param name="force">used for the acceleration in that direction</param>
        public void moveLeft(int force)
        {
            AccelX = -force;
        }

        /// <summary>
        /// Draws the hero object
        /// </summary>
        /// <param name="spriteBatch">an already begun spritebatch</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Rec, source, Color.White);
        }
    }
}
