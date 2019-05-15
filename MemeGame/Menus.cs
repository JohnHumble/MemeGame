using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemeGame
{
    class Menus
    {
        SpriteFont buttonFont;
        Texture2D buttonTexture;

        Button play;

        public Menus(SpriteFont buttonFont, Texture2D buttonTexture)
        {
            this.buttonFont = buttonFont;
            this.buttonTexture = buttonTexture;
            play = new Button(40, 40, 100, 40, "play", buttonFont, buttonTexture);
        }

        public Screen Update(MouseState mouse)
        {
            if (play.IsPressed(mouse))
            {
                return Screen.Play;
            }

            return Screen.Menu;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            play.Draw(spriteBatch);
        }
    }
}
