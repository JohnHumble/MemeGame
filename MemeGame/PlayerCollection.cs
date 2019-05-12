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
    class PlayerCollection : List<Player>
    {
        List<Texture2D> textures;
        int speed, jump, width,height;

        public PlayerCollection(List<Texture2D> textures,int player_width,int player_height,int speed = 5, int jump = 20)
        {
            this.textures = textures;
            this.speed = speed;
            this.jump = jump;
            width = player_width;
            height = player_height;
        }

        public void AddPlayer(Point start_location, Heros type, string name, Color color, Keys left, Keys right, Keys jump)
        {
            Player next = new Player(start_location, width, height, textures[(int)type], left, right, jump, name,color);
            Add(next);

        }

        public void Update(int gravity, WallCollection walls)
        {
            foreach (var player in this)
            {
                player.Update(gravity, walls, jump, speed);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var player in this)
            {
                player.Draw(spriteBatch);
            }
        }

        public void DrawNames(SpriteBatch spriteBatch, SpriteFont font)
        {
            foreach(var player in this)
            {
                player.DrawName(spriteBatch, font);
            }
        }
    }
}
