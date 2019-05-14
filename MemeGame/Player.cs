using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemeGame
{
    class Player
    {
        Hero hero;
        readonly Keys left, right, jump, fire;

        string name;
        Color color;

        public Player(Point StartLocation, int width, int height, Texture2D texture, Keys left, Keys right, Keys jump, Keys shoot, string name, Color color)
        {
            hero = new Hero(StartLocation, width, height, 60, texture);
            this.left = left;
            this.right = right;
            this.jump = jump;
            this.fire = shoot;
            this.name = name;
            this.color = color;
        }

        private void TestInput(int jump_force, int speed)
        {
            if (Keyboard.GetState().IsKeyDown(fire))
            {
                hero.fire();
            }
            if (Keyboard.GetState().IsKeyDown(jump))
            {
                hero.jump(jump_force);
            }
            if (Keyboard.GetState().IsKeyDown(left))
            {
                hero.moveLeft(speed);
            }
            if (Keyboard.GetState().IsKeyDown(right))
            {
                hero.moveRight(speed);
            }
            if (Keyboard.GetState().IsKeyUp(left) && Keyboard.GetState().IsKeyUp(right))
            {
                hero.stop();
            }
        }

        public void Update(int gravity, WallCollection walls,PlayerCollection players,WeaponCollection weapons, int jump_force, int speed)
        {
            TestInput(jump_force,speed);

            hero.Update(gravity, walls,players);

            if (hero.weapon == null)
            {
                foreach(var weapon in weapons)
                {
                    int dis = Tool.distance(hero.GetPoint(), weapon.GetPoint());
                    if (dis < 100)
                    {
                        hero.pickup(weapon);
                        weapons.Remove(weapon);
                        break;
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            hero.Draw(spriteBatch);
        }

        public void DrawName(SpriteBatch spriteBatch, SpriteFont font)
        {
            spriteBatch.DrawString(font, name, new Vector2(hero.HitBox.Center.X - (name.Length/2 * 15), hero.HitBox.Y - 56), color);
        }

        public Hero GetHero()
        {
            return hero;
        }

        /// <summary>
        /// returns a vector for the location of the hero
        /// </summary>
        /// <returns></returns>
        public Vector2 GetLocation()
        {
            return new Vector2(hero.HitBox.Center.X, hero.HitBox.Center.Y);
        }

        /// <summary>
        /// returns a Point value for the location
        /// </summary>
        /// <returns></returns>
        public Point GetPosition()
        {
            return hero.HitBox.Center;
        }
    }
}
