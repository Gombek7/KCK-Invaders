using System;
using System.Collections.Generic;
using System.Text;

namespace Kck_projekt_1.Models
{
    class PlayerProjectile : Projectile
    {
        public PlayerProjectile(Vector2Int coords) : base(coords)
        {
            direction = new Vector2Int(0, -1);
            moveFrameDelay = 0;
            Hitbox = new Hitbox() {UpperLeftCorner = new Vector2Int(0,-1), RightDownCorner = new Vector2Int(0, 1) };
        }
    }
}
