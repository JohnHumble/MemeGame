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
        Button build;

        public Menus(SpriteFont buttonFont, Texture2D buttonTexture)
        {
            this.buttonFont = buttonFont;
            this.buttonTexture = buttonTexture;

            play = new Button(300, 200, 100, 40, "play", buttonFont, buttonTexture);
            build = new Button(300, 300, 100, 40, "build", buttonFont, buttonTexture);
        }

        public Screen Update(MouseState mouse)
        {
            if (play.IsPressed(mouse))
            {
                return Screen.Play;
            }

            if (build.IsPressed(mouse))
            {
                return Screen.Build;
            }

            return Screen.Menu;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            play.Draw(spriteBatch);
            build.Draw(spriteBatch);
        }
    }
}
