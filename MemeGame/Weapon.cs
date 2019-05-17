﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemeGame
{
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
        
        public abstract void Fire(Hero owner);
        public abstract void Update(Hero owner, WallCollection walls, PlayerCollection players);
        //public abstract void Update();

        public abstract void Draw(SpriteBatch spriteBatch);

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
