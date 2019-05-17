using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemeGame
{
    /// <summary>
    /// Abstract class for all weapons.
    /// </summary>
    abstract class Weapon
    {
        public Rectangle rectangle;
        protected Rectangle source;
        protected Texture2D texture;

        public int Damage { get; set; }
        
        public Weapon(Rectangle rectangle, Texture2D texture, int damage)
        {
            this.rectangle = rectangle;
            this.texture = texture;
            Damage = damage;
            source = new Rectangle(0, 0, 120, 60);
        }
        public Weapon()
        {
            Damage = 0;
            rectangle = new Rectangle();
            texture = null;
            source = new Rectangle(0,0,120,60);
        }
        /// <summary>
        /// This is the method that will be called when a player pushes the attack button
        /// </summary>
        /// <param name="owner">hero of the player</param>
        public abstract void Fire(Hero owner);

        /// <summary>
        /// This method is used to update anything needed with the weapon like location.
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="walls"></param>
        /// <param name="players"></param>
        public abstract void Update(Hero owner, WallCollection walls, PlayerCollection players);

        /// <summary>
        /// This method is used to draw the weapon and any other things needed.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public abstract void Draw(SpriteBatch spriteBatch);

        /// <summary>
        /// This method will simply draw the weapon but nothing else.
        /// </summary>
        /// <param name="spriteBatch"></param>
        protected void DrawWeapon(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rectangle, source, Color.White);
        }

        public Point GetPoint()
        {
            return new Point(rectangle.X, rectangle.Y);
        }
    }
}
