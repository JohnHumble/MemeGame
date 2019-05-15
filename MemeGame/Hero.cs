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
        const int IMAGE_WIDTH = 80; //size of each image in the image sheet.
        const int IMAGE_HEIGHT = 120;
        const int MAX_SPEED = 8; // the maximum velocity on the ground.
        const int DAMPEN = 2; // amount to slow down on ground.
        const int CLIP_DIVISOR = 4; // inversly proportional to the amout of clipping

        // location and physics based
        public Rectangle HitBox { get; private set; }
        public Point Velocity { get; private set; }
        public int AccelX { get; set; }
        public int AccelY { get; set; }
        public bool OnGround { get; private set; }
        public bool IsRight { get; private set; }

        // Rendering
        private readonly Texture2D texture;
        private Rectangle source;
        private Rectangle renderRec;
        private int animationFrame;
        private readonly int hit_buffer_x;
        private readonly int hit_buffer_y;

        // game values
        public int Health { get; private set; }
        public Weapon weapon;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="location">start location of the hero</param>
        /// <param name="width">width of the hero</param>
        /// <param name="height">height of the hero</param>
        /// <param name="health">the amout of hp for the hero</param>
        /// <param name="texture">the texture of the hero</param>
        /// <param name="acceleration">Starting acceleration(gravity)</param>
        public Hero(Point location, int width, int height, int health, Texture2D texture)
        {
            animationFrame = 0;
            hit_buffer_x = width / CLIP_DIVISOR;
            hit_buffer_y = height / CLIP_DIVISOR;

            renderRec = new Rectangle(location, new Point(width, height));
            HitBox = new Rectangle(renderRec.X + hit_buffer_x, renderRec.Y + hit_buffer_y, renderRec.Width - hit_buffer_x / 2, renderRec.Height - hit_buffer_y);
            source = new Rectangle(0, 0, IMAGE_WIDTH, IMAGE_HEIGHT);
            this.texture = texture;
            Health = health;
            AccelX = 0;
            AccelY = 0;
            OnGround = false;

            weapon = null;
        }

        public bool Update(int gravity, WallCollection walls, PlayerCollection players)
        {
            AccelY = gravity;

            // Save Velocity and Rec values to be manipulated
            Rectangle rectangle = HitBox;
            Point velocity = Velocity;

            // update velocity based on acceeleration
            velocity.X += AccelX;
            velocity.Y += AccelY;

            if (velocity.X > MAX_SPEED)
            {
                velocity.X = MAX_SPEED;
            }
            else if (velocity.X < -MAX_SPEED)
            {
                velocity.X = -MAX_SPEED;
            }

            if (OnGround)
            {
                velocity.Y = 0;
                if (AccelX == 0)
                {
                    if (velocity.X > DAMPEN)
                    {
                        velocity.X -= DAMPEN;
                    }
                    else if (velocity.X < -DAMPEN)
                    {
                        velocity.X += DAMPEN;
                    }
                    else
                    {
                        velocity.X = 0;
                    }
                }
            }

            // test to see if on the ground
            OnGround = false;
            if (walls.Intersects(new Rectangle(HitBox.X, HitBox.Bottom, HitBox.Width, 1)) != new Rectangle())
            {
                OnGround = true;
            }

            // test to see if on the ground
            Rectangle test;
            Rectangle wall = new Rectangle();
            Point offset = new Point(0);
            // Test Y
            if (velocity.Y > 0) // fall on v
            {
                test = new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height + velocity.Y);
                wall = walls.Intersects(test);
                if (wall != new Rectangle())
                {
                    velocity.Y = wall.Top - rectangle.Bottom;
                }
            }
            else if (velocity.Y < 0) // hit from top ^
            {
                test = new Rectangle(rectangle.X, rectangle.Y + velocity.Y, rectangle.Width, rectangle.Height - velocity.Y );
                wall = walls.Intersects(test);
                if (wall != new Rectangle())
                {
                    velocity.Y = 0;
                }
            }

            rectangle.Y += velocity.Y;
            
            // Test X
            if (velocity.X > 0) // hit from right side ->|
            {
                test = new Rectangle(rectangle.X, rectangle.Y, rectangle.Width + velocity.X, rectangle.Height/2);
                wall = walls.Intersects(test);
                if (wall != new Rectangle())
                {
                    velocity.X = -rectangle.Right + wall.Left;
                    AccelX = 0;
                }
                else
                {
                    // test for moveing over an incline
                    test = new Rectangle(rectangle.X, rectangle.Y, rectangle.Width + velocity.X, rectangle.Height);
                    wall = walls.Intersects(test);
                    if (wall != new Rectangle())
                    {
                        test = new Rectangle(rectangle.X, rectangle.Y - wall.Height, rectangle.Width + velocity.X, rectangle.Height);
                        test = walls.Intersects(test);
                        if (test == new Rectangle())
                        {
                            offset.Y -= wall.Height;
                        }
                        else
                        {
                            velocity.X = -rectangle.Right + wall.Left;
                            AccelX = 0;
                        }
                    }
                }
            }
            else if (velocity.X < 0) // hit from right side |<-
            {
                test = new Rectangle(rectangle.X + velocity.X, rectangle.Y, rectangle.Width - velocity.X, rectangle.Height/2);
                wall = walls.Intersects(test);
                if (wall != new Rectangle())
                {
                    velocity.X = wall.Right - rectangle.Left;
                    AccelX = 0;
                }
                else
                {
                    // test for moveing over an incline
                    test = new Rectangle(rectangle.X + velocity.X, rectangle.Y, rectangle.Width - velocity.X, rectangle.Height);
                    wall = walls.Intersects(test);
                    if (wall != new Rectangle())
                    {
                        test = new Rectangle(rectangle.X + velocity.X, rectangle.Y - wall.Height, rectangle.Width - velocity.X, rectangle.Height);
                        test = walls.Intersects(test);
                        if (test == new Rectangle())
                        {
                            offset.Y -= wall.Height;
                        }
                        else
                        {
                            velocity.X = wall.Right - rectangle.Left;
                            AccelX = 0;
                        }
                    }
                }
            }

            // update weapon if exists
            if (weapon != null)
            {
                weapon.Update(this, walls, players);
            }

            // uppdate the rectangle based on velocity
            rectangle.Y += offset.Y;
            rectangle.X += velocity.X + offset.X;
            
            // reset Velocity and Rec values
            Velocity = velocity;
            HitBox = rectangle;

            return Health > 0;
        }

        public void pickup(Weapon weapon)
        {
            this.weapon = weapon;
        }

        public void drop(WeaponCollection weapons)
        {
            if (weapon != null)
            {
                weapons.Add(weapon);
                weapon = null;
            }
        }

        public void fire()
        {
            if (weapon != null)
            {
                weapon.Fire(this);
            }
        }

        /// <summary>
        /// Method used to have the hero jump. It will only jump if it is on the ground.
        /// </summary>
        /// <param name="force">value of velocity to initially start moving up.</param>
        public void jump(int force)
        {
            if (OnGround)
            {
                Velocity = new Point(Velocity.X, -force);
                OnGround = false;
            }
        }

        /// <summary>
        /// Method used to move the hero to the right.
        /// </summary>
        /// <param name="force">used for acceleration in that direction</param>
        public void moveRight(int force)
        {
            AccelX = force;
            IsRight = true;
        }

        /// <summary>
        /// Method used to move the hero to the left
        /// </summary>
        /// <param name="force">used for the acceleration in that direction</param>
        public void moveLeft(int force)
        {
            AccelX = -force;
            IsRight = false;
        }

        /// <summary>
        /// sets sideways speed to 0
        /// </summary>
        public void stop()
        {
            AccelX = 0;
        }

        /// <summary>
        /// Draws the hero object
        /// </summary>
        /// <param name="spriteBatch">an already begun spritebatch</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            renderRec.X = HitBox.X - hit_buffer_x;
            renderRec.Y = HitBox.Y - hit_buffer_y;

            int frameSpeed = 5;


            if (animationFrame == frameSpeed)
            {
                source.Y = source.Y + IMAGE_HEIGHT;
                animationFrame = 0;
            }
            if (source.Y / IMAGE_HEIGHT >= 9)
                source.Y = IMAGE_HEIGHT;

            if (Velocity.X > 0)
            {
                source.X = IMAGE_WIDTH;
            }
            else if (Velocity.X < 0)
            {
                source.X = 0;
            }
            else
            {
                animationFrame = 0;
                source.Y = 0;
            }
            spriteBatch.Draw(texture, renderRec, source, Color.White);

            animationFrame++;

            if (weapon != null)
            {
                weapon.Draw(spriteBatch);
            }

            // DELET THIS DEBUGING FOR COLLITIONS
    /*        Rectangle test,rectangle, whole;
            Point velocity = Velocity;
            rectangle = Rec;
            if (Velocity.X > 0)
            {
                test = new Rectangle(rectangle.X, rectangle.Y, rectangle.Width + velocity.X, rectangle.Height / 2);
                whole = new Rectangle(rectangle.X, rectangle.Y, rectangle.Width + velocity.X, rectangle.Height);
            }
            else
            {
                test = new Rectangle(rectangle.X + velocity.X, rectangle.Y, rectangle.Width - velocity.X, rectangle.Height/2);
                whole = new Rectangle(rectangle.X + velocity.X, rectangle.Y, rectangle.Width - velocity.X, rectangle.Height);
            }
            spriteBatch.Draw(fill, whole, source, Color.Blue);
            spriteBatch.Draw(fill, test, source, Color.Red);
            */
        }

        public Point GetPoint()
        {
            return new Point(HitBox.X, HitBox.Y);
        }

        public bool TestHit(Rectangle other, int damage)
        {
            if (other.Intersects(HitBox))
            {
                Health -= damage;
                return true;
            }

            return false;
        }
    }
}
