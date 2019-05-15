using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemeGame
{
    class Gun : Weapon
    {
        List<Shot> shot;
        Texture2D shotTexture;

        Point shotLocation;

        private int shotSpeed;
        private int loadShotSpeed;
        private int shotRate;
        private int lastShot;

        public Gun(Rectangle rectangle, Texture2D gun_texture, Texture2D shot_texture, int damage, int shotSpeed = 42, int shotRate = 10)
        {
            this.texture = gun_texture;
            this.rectangle = rectangle;
            Damage = damage;
            shotTexture = shot_texture;
            shot = new List<Shot>();
            shotLocation = new Point(0, 0);

            this.shotSpeed = shotSpeed;
            this.shotRate = shotRate;
            lastShot = 0;
        }

        public override void Fire()
        {
            if (lastShot <= 0)
            {
                shot.Add(new Shot(shotTexture, new Rectangle(shotLocation, new Point(32, 8)), Damage,loadShotSpeed));
                lastShot = shotRate;
            }
            
       //     throw new NotImplementedException();
        }

        public override void Update(Hero owner, WallCollection walls, PlayerCollection players)
        {
            if (owner == null)
            {
                return;
            }

            // set the gun rectangle and start point
            if (owner.IsRight)
            {
                rectangle.X = owner.HitBox.Right;
                rectangle.Y = owner.HitBox.Center.Y;
                shotLocation = new Point(owner.HitBox.Right, rectangle.Y);
                loadShotSpeed = shotSpeed;
            }
            else
            {
                rectangle.X = owner.HitBox.Left - rectangle.Width;
                rectangle.Y = owner.HitBox.Center.Y;
                shotLocation = new Point(owner.HitBox.Left, rectangle.Y);
                loadShotSpeed = -shotSpeed;
            }

            for (int i = 0; i < shot.Count; i++)
            {
                if(shot[i].Update(players, walls))
                {
                    shot.RemoveAt(i);
                    i--;
                }
            }

            // update shot varialbes
            if (lastShot > 0)
            {
                lastShot--;
            }

          //  throw new NotImplementedException();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            DrawWeapon(spriteBatch);
            foreach (var bul in shot)
            {
                bul.Draw(spriteBatch);
            }
            //throw new NotImplementedException();
        }

    }
}
