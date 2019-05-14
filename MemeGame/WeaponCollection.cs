using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemeGame
{
    class WeaponCollection : List<Weapon>
    {
        public WeaponCollection()
        {
            
        }

        public void Update(WallCollection walls, PlayerCollection players)
        {
            foreach (var weapon in this)
            {
                weapon.Update(null, walls, players);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var weapon in this)
            {
                weapon.Draw(spriteBatch);
            }
        }
    }
}
