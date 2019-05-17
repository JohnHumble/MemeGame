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
        Random random;

        // look at ray tracing instead of projectile 
        private int shotSpeed;
        private int loadShotSpeed;
        private int shotRate;
        private int lastShot;
        private int shotSize;
        private int spread;

        public Gun(Rectangle rectangle, Texture2D gun_texture, Texture2D shot_texture, int damage, int shotSpeed = 64, int shotRate = 10, int spread = 3)
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
            shotSize = shotSpeed;
            this.spread = spread;

            random = new Random();
        }

        public override void Fire(Hero owner)
        {
            if (lastShot <= 0)
            {
               // shot.Add(new Shot(shotTexture, new Rectangle(shotLocation, new Point(shotSize, 8)), Damage,loadShotSpeed));
                int y = random.Next(-spread, spread + 1);
                shot.Add(new Shot(shotTexture,shotLocation,new Point(loadShotSpeed,y),owner,8,Damage));
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
                rectangle.X = owner.HitBox.Right - rectangle.Width/2;
                rectangle.Y = owner.HitBox.Y + rectangle.Height/2;
                shotLocation = new Point(owner.HitBox.Center.X, rectangle.Center.Y);
                loadShotSpeed = shotSpeed;

                // TODO change source rectangle to point right
            }
            else
            {
                rectangle.X = owner.HitBox.Left - rectangle.Width;
                rectangle.Y = owner.HitBox.Y + rectangle.Height / 2;
                shotLocation = new Point(owner.HitBox.Center.X - shotSpeed, rectangle.Center.Y);
                loadShotSpeed = -shotSpeed;

                // TODO change source rectangle to point left
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
