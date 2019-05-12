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
        Keys left, right, jump;

        public Player(Point StartLocation, int width, int height, Texture2D texture, Keys left, Keys right, Keys jump)
        {
            hero = new Hero(StartLocation, width, height, 60, texture);
            this.left = left;
            this.right = right;
            this.jump = jump;
        }

        private void testInput(int jump_force, int speed)
        {
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

        public void Update(int gravity, WallCollection walls, int jump_force, int speed)
        {
            testInput(jump_force,speed);

            hero.Update(gravity, walls);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            hero.Draw(spriteBatch);
        }
    }
}
