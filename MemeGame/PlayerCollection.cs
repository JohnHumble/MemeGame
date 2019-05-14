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
        readonly List<Texture2D> textures;
        readonly int speed, jump, width, height;

        public PlayerCollection(List<Texture2D> textures,int player_width,int player_height,int speed = 5, int jump = 28)
        {
            this.textures = textures;
            this.speed = speed;
            this.jump = jump;
            width = player_width;
            height = player_height;
        }

        public void AddPlayer(Point start_location, Heros type, string name, Color color, Keys left, Keys right, Keys jump, Keys fire)
        {
            Player next = new Player(start_location, width, height, textures[(int)type], left, right, jump,fire, name,color);
            Add(next);

        }

        public void Update(int gravity, WallCollection walls, PlayerCollection players, WeaponCollection weapons)
        {
            foreach (var player in this)
            {
                player.Update(gravity, walls, players, weapons, jump, speed);
            }
        }

        public Vector2 GetPlayerMid()
        {
            Vector2 mid = new Vector2(0, 0);
            foreach (var player in this)
            {
                mid += player.GetLocation();
            }

            mid.X /= Count;
            mid.Y /= Count;

            return mid;
        }
        
        public Vector2 GetDifference()
        {
            float maxY = int.MinValue;
            float maxX = int.MinValue;
            float minY = int.MaxValue;
            float minX = int.MaxValue;
            foreach (var player in this)
            {
                maxX = Math.Max(maxX, player.GetLocation().X);
                maxY = Math.Max(maxY, player.GetLocation().Y);
                minX = Math.Min(minX, player.GetLocation().X);
                minY = Math.Min(minY, player.GetLocation().Y);
            }
            return new Vector2(Math.Abs(maxX - minX), Math.Abs(maxY - minY));
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
