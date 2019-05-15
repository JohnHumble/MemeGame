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
    class Button
    {
        Rectangle rectangle;
        Texture2D texture;
        SpriteFont font;
        string text;
        Color color;

        public Button(int x, int y, int width, int height, string text,SpriteFont font,Texture2D texture)
        {
            rectangle = new Rectangle(x, y, width, height);
            this.text = text;
            this.font = font;
            this.texture = texture;
            color = Color.Orange;
        }

        public bool IsPressed(MouseState mouse)
        {
            Point mousePos = new Point(mouse.X, mouse.Y);
                
            if (rectangle.Contains(mousePos))
            {
                color = Color.White;
                return mouse.LeftButton == ButtonState.Pressed;
            }
            else
            {
                color = Color.Orange;
            }
            
            return false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 textLocation = new Vector2(rectangle.X, rectangle.Y);
            spriteBatch.Draw(texture, rectangle, color);
            spriteBatch.DrawString(font, text, textLocation, Color.Black);
        }
    }
}
